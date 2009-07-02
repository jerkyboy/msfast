//=======================================================================
/* Project: MSFast (MySpace.MSFast.SuProxy)
*  Original author: Yadid Ramot (e.yadid@gmail.com)
*  Copyright (C) 2009 MySpace.com 
*
*  This file is part of MSFast.
*  MSFast is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*
*  MSFast is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
* 
*  You should have received a copy of the GNU General Public License
*  along with MSFast.  If not, see <http://www.gnu.org/licenses/>.
*/
//=======================================================================

//Imports
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using MySpace.MSFast.SuProxy.Proxlets;
using MySpace.MSFast.SuProxy.Exceptions;
using MySpace.MSFast.Core.Logger;

namespace MySpace.MSFast.SuProxy.Proxy
{
    public class SuProxyServer
    {
        private static readonly MSFastLogger log = MSFastLogger.GetLogger(typeof(SuProxyServer));

        public delegate void SuProxyEvent(SuProxyServer server);

        private Socket m_serverSocket = null;
        private SuProxyConfiguration configuration = null;

        private bool isCloseRequested = false;
        private Object socketLock = new Object();

        private ProxletsPool proxletsPool = null;

        public SuProxyServer(SuProxyConfiguration config)
        {
            if (config == null)
            {
                throw new InvalidConfigException();
            }
            this.configuration = config;
            this.proxletsPool = new ProxletsPool(config);
        }


        public void Start()
        {
            lock (socketLock)
            {
                isCloseRequested = false;

                if (this.m_serverSocket != null && this.m_serverSocket.IsBound)
                    throw new ProxyAlreadyStartedException();
#if (DEBUG)
                log.Debug("Opening proxy socket, binding and start listening...");
#endif
                this.m_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.m_serverSocket.Bind(new IPEndPoint(IPAddress.Any, configuration.ProxyPort));
                this.m_serverSocket.Listen(configuration.MaxConnectionsQue);
                this.m_serverSocket.Blocking = false;

                BeginAccept();
#if (DEBUG)
                log.Info(String.Format("Proxy Started, Listening on port ({0}), Max Connections Que: {1}, Proxlets Pool Size: {2}", configuration.ProxyPort, configuration.MaxConnectionsQue, configuration.ProxletsPoolSize));
#endif
            }
        }

        public void Stop()
        {
            lock (socketLock)
            {
                if (this.m_serverSocket == null || this.m_serverSocket.IsBound == false)
                    throw new ProxyIsInactiveException();
#if (DEBUG)
                log.Debug("Closing proxy socket...");
#endif
                this.m_serverSocket.Close();
                this.m_serverSocket = null;

                this.proxletsPool.KillProxlets();

#if (DEBUG)
                log.Info("Proxy Closed!");
#endif
            }
        }

        public void Restart()
        {
            lock (socketLock)
            {
                try
                {
                    Stop();
                }
                catch (ProxyIsInactiveException)
                { }

                Start();
            }
        }

        public void Dispose()
        {
            try
            {
                Stop();
            }
            catch (ProxyIsInactiveException) { }
            try
            {
                this.proxletsPool.Dispose();
            }
            catch { }
        }
        #region Socket Actions

        private void OnClientConnect(IAsyncResult asyn)
        {
            if (isCloseRequested)
                return;

            ///TODO: Create factory that detects the connection type, then calls the right "Proxlet" pool (right now its only HTTP)
            Socket s = EndAccept(asyn);

            if (s != null)
            {
                Proxlet p = proxletsPool.GetProxlet();

                if (p != null)//drop connection if null
                {
                    p.Process(s);
                }
            }

            BeginAccept();
        }

        private void BeginAccept()
        {
            lock (socketLock)
            {
                if (isCloseRequested || m_serverSocket == null)
                    return;

                try
                {
                    m_serverSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                }
                catch
                {
                }
            }
        }

        private Socket EndAccept(IAsyncResult asyn)
        {
            lock (socketLock)
            {
                if (isCloseRequested || m_serverSocket == null)
                    return null;

                try
                {
                    return m_serverSocket.EndAccept(asyn);
                }
                catch (ArgumentException)
                {
                }
                catch
                {
                }
            }
            return null;
        }

        #endregion


    }
}
