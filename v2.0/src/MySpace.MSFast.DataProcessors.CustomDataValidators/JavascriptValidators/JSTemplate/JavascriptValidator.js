
load(BASE_FOLDER + "/JSTemplate/Imports.js");

function JavascriptValidator()
{
    if (processedDataPackage == undefined)
        throw "Invalid Data";

    if (Validate == undefined)
        throw "Invalid Validator";

    processedDataPackage.log.pages[0].pageSource = Base64.decode(processedDataPackage.log.pages[0].pageSource);

    var results = Validate(processedDataPackage, EXTDATA);

    if (results == undefined)
        throw "Invalid Results";

    print("RESULTS[" + results.score + ":" + results.occurrences.length + "]");

    if (results.occurrences.length > 0)
    {
        for (var i = 0; i < results.occurrences.length; i++)
        {
            if (results.occurrences[i].TYPE == "SourceValidationOccurance") {
                print("SOURCE[" + results.occurrences[i].startIndex + ":" + results.occurrences[i].length + "]");
            }
            else if (results.occurrences[i].TYPE == "DownloadStateOccurance") {
                print("DOWNLOAD[" + results.occurrences[i].uRL + "]");
            }
        }
    }
}

var j = new JavascriptValidator();