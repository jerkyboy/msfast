$.fn.extend({
    setWizardPage: function(id) {return this.trigger("setWizardPage", id);},
    nextWizardPage: function() {return this.trigger("nextWizardPage");},
    prevWizardPage: function() {return this.trigger("prevWizardPage");},
    saveAllWizardPages: function(cb) {return this.trigger("saveAllWizardPages", cb);}
});
        
$.fn.wizard = function(props){
    props = $.extend({defaultPage : 0, saveBetweenChanges: false, pages : []}, props);
    var currentPage = props.defaultPage;    
    var inst = this;    
    var showpage = function(wid){
        var tmp = function(){
            currentPage = wid;
            $(inst).trigger("onWizardPageSwitched", {index : wid, page : props.pages[wid], hasNext : (currentPage <  props.pages.length-1), hasPrev : (currentPage > 0)});
        }
        var ht = props.pages[wid].container.html();
        if(!ht && props.pages[wid].getContentURL){
            $(inst).trigger("onWizardLoadingPage", {index : wid, page : props.pages[wid]});            
            props.pages[wid].container.loadControl(props.pages[wid].getContentURL(), function(){
                tmp();
            });
        }else{
            tmp();
        }
    }       
    $(this)
    .bind("nextWizardPage",function(e,a){
        if(currentPage <  props.pages.length-1) 
          $(this).setWizardPage(currentPage+1);
    })
    .bind("prevWizardPage",function(e,a){
        if(currentPage > 0) 
          $(this).setWizardPage(currentPage-1);
    })
    .bind("setWizardPage",function(e, id){
        $(this).trigger("onBeforeSwitchPage");
        if(id >=  props.pages.length || id < 0) return;
        if(props.saveBetweenChanges){
            $(this).saveAllWizardPages(function(status){
                if(status.success) showpage(id);
                else  showpage(status.failedOn);
            });
        }else{
            showpage(id);
        }
    }).bind("saveAllWizardPages",function(e,cb){
        var p = -1;
        var saveLst = [];
        for(var i = 0 ; i < props.pages.length; i++){
            if(props.pages[i].dependencies){
                for(var d = 0 ; d < props.pages[i].dependencies.length; d++){
                    saveLst.push({name : props.pages[i].dependencies[d], page : props.pages[i],index : i});        
                }    
            }
            saveLst.push({name : props.pages[i].name, page : props.pages[i],index : i});
        }
        var fnc = function(ok){
            if(!ok){
                if(cb) cb({failedOn: saveLst[p].index});
                showpage(saveLst[p].index);
                return;    
            }
            p++;            
            if(p >= saveLst.length){
                if(cb) cb({success: true});
                return;
            }            
            var e = {name : saveLst[p].name, callback : fnc, caught : false};            
            $(document).trigger("savePage", e);            
            if(!e.caught) fnc(true);
        }
        fnc(true);
    });
    $(this).setWizardPage(props.defaultPage);
    return this;
};