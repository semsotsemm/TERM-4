var promises = [
    Promise.resolve("Успех 1"),
    Promise.reject("Ошибка A"),
    Promise.resolve("Успех 2"),
    Promise.reject("Ошибка B"),
    Promise.resolve("Успех 3")
];
Promise.allSettled(promises).then(function (results) {
    console.log("--- Только успешные результаты ---");
    results.forEach(function (result) {
        if (result.status === "fulfilled") {
            console.log("\u041F\u043E\u043B\u0443\u0447\u0435\u043D\u043E \u0437\u043D\u0430\u0447\u0435\u043D\u0438\u0435: ".concat(result.value));
        }
    });
});
