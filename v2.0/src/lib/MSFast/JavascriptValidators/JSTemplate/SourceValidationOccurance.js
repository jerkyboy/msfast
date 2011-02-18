function SourceValidationOccurance( _comment,
                                    _helpURL,
                                    _startIndex,
                                    _length,
                                    _sourceData)
{
    this.TYPE = "SourceValidationOccurance";
    this.comment = _comment;
    this.helpURL = _helpURL;
    this.startIndex = _startIndex;
    this.length = _length;
    this.sourceData = _sourceData;

    this.peiceSource = function(){
        if (this.sourceData == null || this.sourceData == undefined || this.sourceData == "" ||
			this.sourceData.length < this.startIndex + this.length)
			return "";
        return this.sourceData.substr(this.startIndex, this.length);
    };
}
