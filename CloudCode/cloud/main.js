var objectId;
var username;
var replacedByUsername;
var selectedDate;
var installationId;

/////////before save validation
Parse.Cloud.beforeSave("Abdication", function(request, response) {
    getRequestData(request);
    
    handleSpotClaim().then(checkAvailability).then(
        function(){
            response.success();
        },
        function(message){
            response.error(message);
        }
    );
});

function handleSpotClaim(){
    var savedAbdicationQuery = new Parse.Query("Abdication");
    savedAbdicationQuery.equalTo("objectId",objectId);
    return savedAbdicationQuery.find();    
}

function checkAvailability(results){
    var promise = new Parse.Promise();
    
    if(results.length > 0){
        var savedAbdication = results[0];
        var replacedBy = savedAbdication.get("ReplacedByUsername");
        if(replacedBy){
            var message = "You're late! " + replacedBy + " took the spot!";
            sendPushToSenderOnly(true,message).then(function(){                
                promise.reject(message);               
            });
        }
        else {
            promise.resolve();
        }
    }
    else {
        promise.resolve();
    }
    
    return promise;
}

/////////After save
Parse.Cloud.afterSave("Abdication", function(request) {
    getRequestData(request);
    
    var message;
    if(replacedByUsername){        
        message = replacedByUsername + " took " + username + "'s spot for " + selectedDate + ".";
    }
    else{    
        message = username + " won't use his parking spot on " + selectedDate + ". Respond to this to get it.";
    }
    
    sendPushToSenderOnly(false,message);
});

/////////Common functions
function getRequestData(request){
    var abdication = request.object;
    objectId = abdication.id;
    username = abdication.get("Username");
    replacedByUsername = abdication.get("ReplacedByUsername");
    selectedDate = new Date(abdication.get("SelectedDate")).toLocaleDateString();
    installationId = request.installationId;
}

function sendPushToSenderOnly(senderOnly,message){
    var query = new Parse.Query("_Installation");
    if(senderOnly){
        console.log("install id: " + installationId);
        query.equalTo("installationId",installationId);
    }
    else{//Everyone, but sender
        query.notEqualTo("installationId",installationId);
    }
        
    return Parse.Push.send({
        where: query,
        data:{
            alert: message,
            objectId: objectId
        }
    }); 
}