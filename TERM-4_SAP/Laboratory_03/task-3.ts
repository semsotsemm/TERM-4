const promises = [
    Promise.resolve("Успех 1"),
    Promise.reject("Ошибка A"),
    Promise.resolve("Успех 2"),
    Promise.reject("Ошибка B"),
    Promise.resolve("Успех 3")
];

Promise.allSettled(promises).then((results) => {
    console.log("--- Только успешные результаты ---");
    
    results.forEach((result) => {
        if (result.status === "fulfilled") {
            console.log(`Получено значение: ${result.value}`);
        }
    });
});