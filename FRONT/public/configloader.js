let config = {};

var getConfig = function(key){
    return config[key];
}

var initializeConfig = function() {
    return fetch("/config.json")
    .then(res => res.json())
    .then(data => {
        config=data
        console.log(data);
    });
}