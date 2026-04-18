var fastFetch = function () {
    return new Promise(function (resolve) {
        setTimeout(function () { resolve("фастФеч"); }, 2000);
    });
};
var slowFetch = function () {
    return new Promise(function (resolve) {
        setTimeout(function () {
            resolve("слоуФеч");
        }, 2000);
    });
};
Promise.race([fastFetch(), slowFetch()]).then(function (winner) { console.log(winner); }).catch(function (error) { console.error(error); });
