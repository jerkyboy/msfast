/*
* jQuery Form Plugin
* version: 2.19 (02-FEB-2009)
* @requires jQuery v1.2.2 or later
*
* Examples and documentation at: http://malsup.com/jquery/form/
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*/

; (function($) {
    $.fn.ajaxSubmit = function(options) {
        // fast fail if nothing selected (http://dev.jquery.com/ticket/2752)
        if (!this.length) return this;

        this.removeAttr("onsubmit");

        if (typeof options == 'function') options = { complete: options };

        options = $.extend({
            url: this.attr('action') || window.location.toString(),
            dataType: "json",
            type: this.attr('method') || 'POST',
            autoredirect: true
        }, options || {});

        var results = { frm: this };

        this.bind("beforeSubmit", __beforeSubmit);
        this.bind("formCompleted", __formCompleted);
        this.bind("formPostCompleted", __formPostCompleted);
        
        if (options.autoredirect)
            this.bind("formIndicator", __formIndicator_redirect);

        this.trigger('beforeSerialize', results);

        if (results.kill) {
            this.trigger("formCompleted", results);
            this.trigger("formPostCompleted", results);
            return this;
        }

        var a = this.formToArray(options.semantic);

        if (options.data) {
            options.extraData = options.data;
            for (var n in options.data) {
                if (options.data[n] instanceof Array) {
                    for (var k in options.data[n]) a.push({ name: n, value: options.data[n][k] })
                }
                else
                    a.push({ name: n, value: options.data[n] });
            }
        }

        this.trigger('beforeSubmit', results);

        if (results.kill) {
            this.trigger('formCompleted', results);
            this.trigger("formPostCompleted", results);
            return this;
        }

        var q = $.param(a);

        if (options.type.toUpperCase() == 'GET') {
            options.url += (options.url.indexOf('?') >= 0 ? '&' : '?') + q;
            options.data = null;  // data is null for 'get'
        }
        else options.data = q; // data is the query string for 'post'

        var $form = this, callbacks = [];

        // perform a load on the target only if dataType is not provided
        if (!options.dataType && options.target) {
            var oldComplete = options.complete || function() { };
            callbacks.push(function(data) {
                $(options.target).html(data).each(oldComplete, arguments);
            });
        }
        else
            callbacks.push(function(xhr, data, status, $form) {
                results["xhr"] = xhr;
                results["data"] = data;
                results["passed"] = status;
                $form.trigger('formCompleted', results);
                $form.trigger("formPostCompleted", results);
            });

        options.complete = function() {
            $form.trigger('formReceivedResponse', results);
        }

        options.success = function(data, status) {
            for (var i = 0, max = callbacks.length; i < max; i++)
                callbacks[i].apply(options, [null, data, status == "success", $form]);
        };

        // are there files to upload?
        var files = $('input:file', this).fieldValue();
        var found = false;
        for (var j = 0; j < files.length; j++)
            if (files[j])
            found = true;

        // options.iframe allows user to force iframe mode
        if (options.iframe || found) {
            // hack to fix Safari hang (thanks to Tim Molendijk for this)
            // see:  http://groups.google.com/group/jquery-dev/browse_thread/thread/36395b7ab510dd5d
            if ($.browser.safari && options.closeKeepAlive)
                $.get(options.closeKeepAlive, fileUpload);
            else
                fileUpload();
        }
        else
            $.ajax(options);
        
        // fire 'notify' event
        this.trigger('formSubmitting', results);
        return this;


        // private function for handling file uploads (hat tip to YAHOO!)
        function fileUpload() {
            var form = $form[0];

            if ($(':input[name=submit]', form).length) {
                alert('Error: Form elements must not be named "submit".');
                return;
            }

            var opts = $.extend({}, $.ajaxSettings, options);
            var s = jQuery.extend(true, {}, $.extend(true, {}, $.ajaxSettings), opts);

            var id = 'jqFormIO' + (new Date().getTime());
            var $io = $('<iframe id="' + id + '" name="' + id + '" />');
            var io = $io[0];

            if ($.browser.msie || $.browser.opera)
                io.src = 'javascript:false;document.write("");';

            $io.css({ position: 'absolute', top: '-1000px', left: '-1000px' });

            var xhr = { // mock object
                aborted: 0,
                responseText: null,
                responseXML: null,
                status: 0,
                statusText: 'n/a',
                getAllResponseHeaders: function() { },
                getResponseHeader: function() { },
                setRequestHeader: function() { },
                abort: function() {
                    this.aborted = 1;
                    $io.attr('src', 'about:blank'); // abort op in progress
                }
            };

            var g = opts.global;
            // trigger ajax global events so that activity/block indicators work like normal
            if (g && !$.active++) $.event.trigger("ajaxStart");
            if (g) $.event.trigger("ajaxSend", [xhr, opts]);

            if (s.beforeSend && s.beforeSend(xhr, s) === false) {
                s.global && jQuery.active--;
                return;
            }

            if (xhr.aborted)
                return;

            var cbInvoked = 0;
            var timedOut = 0;

            // add submitting element to data if we know it
            var sub = form.clk;
            if (sub) {
                var n = sub.name;
                if (n && !sub.disabled) {
                    options.extraData = options.extraData || {};
                    options.extraData[n] = sub.value;
                    if (sub.type == "image") {
                        options.extraData[name + '.x'] = form.clk_x;
                        options.extraData[name + '.y'] = form.clk_y;
                    }
                }
            }

            // take a breath so that pending repaints get some cpu time before the upload starts
            setTimeout(function() {
                // make sure form attrs are set
                var t = $form.attr('target'), a = $form.attr('action');

                // update form attrs in IE friendly way

                form.setAttribute('target', id);
                if (form.getAttribute('method') != 'POST')
                    form.setAttribute('method', 'POST');
                if (form.getAttribute('action') != opts.url)
                    form.setAttribute('action', opts.url);

                // ie borks in some cases when setting encoding
                if (!options.skipEncodingOverride) {
                    $form.attr({
                        encoding: 'multipart/form-data',
                        enctype: 'multipart/form-data'
                    });
                }

                // support timout
                if (opts.timeout)
                    setTimeout(function() { timedOut = true; cb(); }, opts.timeout);

                // add "extra" data to form if provided in options
                var extraInputs = [];
                try {
                    if (!options.extraData) {
                        options.extraData = {}
                    }
                    options.extraData["frmifr"] = 1;

                    if (options.extraData)
                        for (var n in options.extraData)
                        extraInputs.push(
                            $('<input type="hidden" name="' + n + '" value="' + options.extraData[n] + '" />')
                                .appendTo(form)[0]);

                    // add iframe to doc and submit the form
                    $io.appendTo('body');
                    io.attachEvent ? io.attachEvent('onload', cb) : io.addEventListener('load', cb, false);
                    form.submit();
                }
                finally {
                    // reset attrs and remove "extra" input elements
                    form.setAttribute('action', a);
                    t ? form.setAttribute('target', t) : $form.removeAttr('target');
                    $(extraInputs).remove();
                }
            }, 10);

            var operaHack = 0;
            function cb() {
                if (cbInvoked++) return;

                io.detachEvent ? io.detachEvent('onload', cb) : io.removeEventListener('load', cb, false);

                var ok = true;

                try {
                    if (timedOut) throw 'timeout';
                    // extract the server response from the iframe
                    var data, doc;

                    doc = io.contentWindow ? io.contentWindow.document : io.contentDocument ? io.contentDocument : io.document;

                    if (doc.body == null && !operaHack && $.browser.opera) {
                        // In Opera 9.2.x the iframe DOM is not always traversable when
                        // the onload callback fires so we give Opera 100ms to right itself
                        operaHack = 1;
                        cbInvoked--;
                        setTimeout(cb, 100);
                        return;
                    }

                    xhr.responseText = doc.body ? doc.body.innerHTML : null;
                    xhr.responseXML = doc.XMLDocument ? doc.XMLDocument : doc;
                    xhr.getResponseHeader = function(header) {
                        var headers = { 'content-type': opts.dataType };
                        return headers[header];
                    };

                    if (opts.dataType == 'json' || opts.dataType == 'script') {
                        var ta = doc.getElementsByTagName('textarea')[0];
                        xhr.responseText = ta ? ta.value : xhr.responseText;
                    }
                    else if (opts.dataType == 'xml' && !xhr.responseXML && xhr.responseText != null) {
                        xhr.responseXML = toXml(xhr.responseText);
                    }
                    data = $.httpData(xhr, opts.dataType);
                }
                catch (e) {
                    ok = false;
                    $.handleError(opts, xhr, 'error', e);
                }

                // ordering of these callbacks/triggers is odd, but that's how $.ajax does it

                if (ok && g) {
                    opts.success(data, ok);
                    $.event.trigger("ajaxSuccess", [xhr, opts]);
                }
                if (g) $.event.trigger("ajaxComplete", [xhr, opts]);
                if (g && ! --$.active) $.event.trigger("ajaxStop");
                if (opts.complete) opts.complete(xhr, data, ok);


                // clean up
                setTimeout(function() {
                    $io.remove();
                    xhr.responseXML = null;
                }, 100);
            };

            function toXml(s, doc) {
                if (window.ActiveXObject) {
                    doc = new ActiveXObject('Microsoft.XMLDOM');
                    doc.async = 'false';
                    doc.loadXML(s);
                }
                else
                    doc = (new DOMParser()).parseFromString(s, 'text/xml');
                return (doc && doc.documentElement && doc.documentElement.tagName != 'parsererror') ? doc : null;
            };

            return this;
        };
    };

    /**
    * ajaxForm() provides a mechanism for fully automating form submission.
    *
    * The advantages of using this method instead of ajaxSubmit() are:
    *
    * 1: This method will include coordinates for <input type="image" /> elements (if the element
    *    is used to submit the form).
    * 2. This method will include the submit element's name/value data (for the element that was
    *    used to submit the form).
    * 3. This method binds the submit() method to the form for you.
    *
    * The options argument for ajaxForm works exactly as it does for ajaxSubmit.  ajaxForm merely
    * passes the options argument along after properly binding events for submit elements and
    * the form itself.
    */
    $.fn.ajaxForm = function(options) {
        return this.ajaxFormUnbind().bind('submit.form-plugin', function() {
            $(this).ajaxSubmit(options);
            return false;
        }).each(function() {
            // store options in hash
            $(":submit,input:image", this).bind('click.form-plugin', function(e) {
                var form = this.form;
                form.clk = this;
                if (this.type == 'image') {
                    if (e.offsetX != undefined) {
                        form.clk_x = e.offsetX;
                        form.clk_y = e.offsetY;
                    } else if (typeof $.fn.offset == 'function') { // try to use dimensions plugin
                        var offset = $(this).offset();
                        form.clk_x = e.pageX - offset.left;
                        form.clk_y = e.pageY - offset.top;
                    } else {
                        form.clk_x = e.pageX - this.offsetLeft;
                        form.clk_y = e.pageY - this.offsetTop;
                    }
                }
                // clear form vars
                setTimeout(function() { form.clk = form.clk_x = form.clk_y = null; }, 10);
            });
        });
    };

    // ajaxFormUnbind unbinds the event handlers that were bound by ajaxForm
    $.fn.ajaxFormUnbind = function() {
        this.unbind('submit.form-plugin');
        return this.each(function() {
            $(":submit,input:image", this).unbind('click.form-plugin');
        });

    };

    /**
    * formToArray() gathers form element data into an array of objects that can
    * be passed to any of the following ajax functions: $.get, $.post, or load.
    * Each object in the array has both a 'name' and 'value' property.  An example of
    * an array for a simple login form might be:
    *
    * [ { name: 'username', value: 'jresig' }, { name: 'password', value: 'secret' } ]
    *
    * It is this array that is passed to pre-submit callback functions provided to the
    * ajaxSubmit() and ajaxForm() methods.
    */
    $.fn.formToArray = function(semantic) {
        var a = [];
        if (this.length == 0) return a;

        var form = this[0];
        var els = semantic ? form.getElementsByTagName('*') : form.elements;
        if (!els) return a;
        for (var i = 0, max = els.length; i < max; i++) {
            var el = els[i];
            var n = el.name;
            if (!n) continue;

            if (semantic && form.clk && el.type == "image") {
                // handle image inputs on the fly when semantic == true
                if (!el.disabled && form.clk == el)
                    a.push({ name: n + '.x', value: form.clk_x }, { name: n + '.y', value: form.clk_y });
                continue;
            }

            var v = $.fieldValue(el, true);
            if (v && v.constructor == Array) {
                for (var j = 0, jmax = v.length; j < jmax; j++)
                    a.push({ name: n, value: v[j] });
            }
            else if (v !== null && typeof v != 'undefined')
                a.push({ name: n, value: v });
        }

        if (!semantic && form.clk) {
            // input type=='image' are not found in elements array! handle them here
            var inputs = form.getElementsByTagName("input");
            for (var i = 0, max = inputs.length; i < max; i++) {
                var input = inputs[i];
                var n = input.name;
                if (n && !input.disabled && input.type == "image" && form.clk == input)
                    a.push({ name: n + '.x', value: form.clk_x }, { name: n + '.y', value: form.clk_y });
            }
        }
        return a;
    };

    /**
    * Serializes form data into a 'submittable' string. This method will return a string
    * in the format: name1=value1&amp;name2=value2
    */
    $.fn.formSerialize = function(semantic) {
        //hand off to jQuery.param for proper encoding
        return $.param(this.formToArray(semantic));
    };

    /**
    * Serializes all field elements in the jQuery object into a query string.
    * This method will return a string in the format: name1=value1&amp;name2=value2
    */
    $.fn.fieldSerialize = function(successful) {
        var a = [];
        this.each(function() {
            var n = this.name;
            if (!n) return;
            var v = $.fieldValue(this, successful);
            if (v && v.constructor == Array) {
                for (var i = 0, max = v.length; i < max; i++)
                    a.push({ name: n, value: v[i] });
            }
            else if (v !== null && typeof v != 'undefined')
                a.push({ name: this.name, value: v });
        });
        //hand off to jQuery.param for proper encoding
        return $.param(a);
    };

    /**
    * Returns the value(s) of the element in the matched set.  For example, consider the following form:
    *
    *  <form><fieldset>
    *      <input name="A" type="text" />
    *      <input name="A" type="text" />
    *      <input name="B" type="checkbox" value="B1" />
    *      <input name="B" type="checkbox" value="B2"/>
    *      <input name="C" type="radio" value="C1" />
    *      <input name="C" type="radio" value="C2" />
    *  </fieldset></form>
    *
    *  var v = $(':text').fieldValue();
    *  // if no values are entered into the text inputs
    *  v == ['','']
    *  // if values entered into the text inputs are 'foo' and 'bar'
    *  v == ['foo','bar']
    *
    *  var v = $(':checkbox').fieldValue();
    *  // if neither checkbox is checked
    *  v === undefined
    *  // if both checkboxes are checked
    *  v == ['B1', 'B2']
    *
    *  var v = $(':radio').fieldValue();
    *  // if neither radio is checked
    *  v === undefined
    *  // if first radio is checked
    *  v == ['C1']
    *
    * The successful argument controls whether or not the field element must be 'successful'
    * (per http://www.w3.org/TR/html4/interact/forms.html#successful-controls).
    * The default value of the successful argument is true.  If this value is false the value(s)
    * for each element is returned.
    *
    * Note: This method *always* returns an array.  If no valid value can be determined the
    *       array will be empty, otherwise it will contain one or more values.
    */
    $.fn.fieldValue = function(successful) {
        for (var val = [], i = 0, max = this.length; i < max; i++) {
            var el = this[i];
            var v = $.fieldValue(el, successful);
            if (v === null || typeof v == 'undefined' || (v.constructor == Array && !v.length))
                continue;
            v.constructor == Array ? $.merge(val, v) : val.push(v);
        }
        return val;
    };

    /**
    * Returns the value of the field element.
    */
    $.fieldValue = function(el, successful) {
        var n = el.name, t = el.type, tag = el.tagName.toLowerCase();
        if (typeof successful == 'undefined') successful = true;

        if (successful && (!n || el.disabled || t == 'reset' || t == 'button' ||
        (t == 'checkbox' || t == 'radio') && !el.checked ||
        (t == 'submit' || t == 'image') && el.form && el.form.clk != el ||
        tag == 'select' && el.selectedIndex == -1))
            return null;

        if (tag == 'select') {
            var index = el.selectedIndex;
            if (index < 0) return null;
            var a = [], ops = el.options;
            var one = (t == 'select-one');
            var max = (one ? index + 1 : ops.length);
            for (var i = (one ? index : 0); i < max; i++) {
                var op = ops[i];
                if (op.selected) {
                    // extra pain for IE...
                    var v = ($.browser.msie && op.attributes && op.attributes['value'] && !(op.attributes['value'].specified)) ? op.text : op.value;
                    if (one) return v;
                    a.push(v);
                }
            }
            return a;
        }
        return el.value;
    };

    /**
    * Checks/unchecks any matching checkboxes or radio buttons and
    * selects/deselects and matching option elements.
    */
    $.fn.selected = function(select) {
        if (select == undefined) select = true;
        return this.each(function() {
            var t = this.type;
            if (t == 'checkbox' || t == 'radio')
                this.checked = select;
            else if (this.tagName.toLowerCase() == 'option') {
                var $sel = $(this).parent('select');
                if (select && $sel[0] && $sel[0].type == 'select-one') {
                    // deselect all other options
                    $sel.find('option').selected(false);
                }
                this.selected = select;
            }
        });
    };


    function __beforeSubmit(e, results) {
        if (results.frm.validateForm) {
            var err = results.frm.validateForm();
            if (err.length > 0) {
                if (results.data == undefined) results.data = { errs: null, msgs: null, cmds: null };
                results.data.errs = err;
                results.kill = true;
                return;
            }
        }
        var ckie = results.frm.find(" input[name='formhash']").val();
        if (ckie) {
            var options = { path: '/', expires: 0 };
            $.cookie("formhash", ckie, options);
            return;
        }
        results.kill = true;
    };

    function __formCompleted(e, results) {
        if(results && results.data && results.data.errs && results.data.errs.length > 0)
            results.frm.find("input[type=password]").val("");
        
        if (results.status && results.data && results.data.hash) {
            results.frm.find("input[name=formhash]").val(data.hash);
        }
        
        if (results.data && results.data.cmds && results.data.cmds.length > 0) {
            for (var i = 0; i < results.data.cmds.length; i++) {
                results.frm.trigger("formIndicator", [results, results.data.cmds[i].key, results.data.cmds[i].val]);
            }
        }
    };
    
    function __formIndicator_redirect(e, results, key, val) {
        if (key == "redirect" && val){
            location.href = (val);
            results.redirectRequested = true;
        }
    };
    
    function __formPostCompleted(e,r){
    }

})(jQuery);

