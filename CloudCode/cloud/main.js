Parse.Cloud.afterSave("Abdication", function(request) {
    var objectId = request.object.id;
    var username = request.object.get("Username");
    var replacedByUsername = request.object.get("ReplacedByUsername");
    var selectedDate = new Date(request.object.get("SelectedDate")).toLocaleDateString();
    
    if(replacedByUsername){
        var message = replacedByUsername + " took " + username + "'s spot for " + selectedDate + ".";
    }
    else{    
        var message = username + " won't use his parking spot on " + selectedDate + ". Respond to this to get it.";
    }
        
    Parse.Push.send({
        channels:[""],
        data:{
            alert: message,
            objectId: objectId
        }
    });
});