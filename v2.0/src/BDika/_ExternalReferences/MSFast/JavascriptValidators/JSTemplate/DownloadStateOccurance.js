
//Const
var URLType = {"Image":1,"CSS":2,"JS":3,"Unknown":4};

function DownloadStateOccurance(_comment,
                                _uRLType,
                                _totalSent,
                                _totalReceived,
                                _sendingRequestStartTime,
                                _sendingRequestEndTime,
                                _receivingResponseStartTime,
                                _receivingResponseEndTime,
                                _connectionStartTime,
                                _connectionEndTime,
                                _uRL,
                                _fileGUID)
{
    this.TYPE = "DownloadStateOccurance";
    this.comment = _comment;
    this.uRLType = _uRLType;
    this.totalSent = _totalSent;
    this.totalReceived = _totalReceived;
    this.sendingRequestStartTime = _sendingRequestStartTime;
    this.sendingRequestEndTime = _sendingRequestEndTime;
    this.receivingResponseStartTime = _receivingResponseStartTime;
    this.receivingResponseEndTime = _receivingResponseEndTime;
    this.connectionStartTime = _connectionStartTime;
    this.connectionEndTime = _connectionEndTime;
    this.uRL = URL;
    this.fileGUID = FileGUID;
}


