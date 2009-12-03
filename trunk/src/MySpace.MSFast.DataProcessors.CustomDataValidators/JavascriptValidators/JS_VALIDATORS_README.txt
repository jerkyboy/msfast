Creating a javascript based validator  
--------------------------------------
1.	Add a new entry in the validators configuration file ("DefaultPageValidation.xml" by default) 
	pointing to the "JavascriptDataValidatorWrapper" validator: 
	
	<validator>
		<groupname>
			[ -- Your validator gourp name -- ]	
		</groupname>
		<name>
			[ -- Your validator name -- ]
		</name>
		<description>
			[ -- Your validator description -- ]
		</description>
		<classname>MySpace.MSFast.DataProcessors.CustomDataValidators.JavascriptValidators.JavascriptDataValidatorWrapper</classname>
		<assembly>MySpace.MSFast.DataProcessors.CustomDataValidators.dll</assembly>
		<data>
			<script><![CDATA[
			
			[ -- Validator script -- ]
			
			]]></script>
		</data>
	</validator>	

2.	The validator script should define a "Validate(_page,_data)" which going to called by the 
	validation engine, where:
		_page - The collected page information as a JSON object with an HAR structure.
		_data - Additional arguments read from the <data> configuration node
		
	more info about the HAR structure is available here - 
	http://groups.google.com/group/firebug-working-group/web/http-tracing---export-format?pli=1
	
		function Validate(_page,_data)
		{
	
		}
		
3.	The "Validate" function should return an object of type "ValidationResults" populated 
	with the validation exceptions results and score.
	The ValidationResults class is already defined by the engine. to instantiate a new object, call:
    
    var results = new ValidationResults();


Example Validator
=============================================================================================
This validator search the page's source code for deprecated domain names defined in the 
<data>/<urls_regex> node and alert the user on each occurence.
---------------------------------------------------------------------------------------------

	<validator>
		<groupname>
			MySpace	
		</groupname>
		<name>
			Deprecated Domains
		</name>
		<description>
			This validator searchs for deprecated domain name references.
		</description>
		<classname>MySpace.MSFast.DataProcessors.CustomDataValidators.JavascriptValidators.JavascriptDataValidatorWrapper</classname>
		<assembly>MySpace.MSFast.DataProcessors.CustomDataValidators.dll</assembly>
		<data>
			<urls_regex>x\.myspacecdn\.com</urls_regex>
			<script><![CDATA[
			
				function Validate(_page,_data)
				{
					var pageSource = _page.log.pages[0].pageSource;

					var results = new ValidationResults();
					results.score = -1;
				    
					if(pageSource){
				        
						var mtch = pageSource.matchs(new RegExp(_data["urls_regex"],"gi"));
				        
						if (mtch != undefined) {
				            
							for (var i = 0; i < mtch.length; i++)
							{
								results.occurrences.push(new SourceValidationOccurance("","", mtch[i].index, mtch[i].length , pageSource));
							}
						}
				        
						if(results.occurrences.length <= 0)
						{
							results.score = 100;
						}
						else if(results.occurrences.length <= 3)
						{
							results.score = 50;
						}
						else
						{
							results.score = 0;
						}
					}
				    
					return results;
				}
							
			]]></script>
		</data>
	</validator>	





