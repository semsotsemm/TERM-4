const checkStock = (item) => {
    return new Promise((resolve, reject) => {
        console.log(`Проверка наличия ${item}`);
        setTimeout(() => {
            const isAvailable = true;
            if (isAvailable) {
                resolve({ item, price: 600 });
            }
            else {
                reject(new Error("Товара нет в наличии"));
            }
        }, 1000);
    });
};
const processPayment = (order) => {
    return new Promise((resolve, reject) => {
        console.log(`Обработка платежа:\nПредмет: ${order.item}\nЗаказ: ${order.price}`);
        setTimeout(() => {
            const balance = 1000;
            if (balance > 500) {
                resolve(order);
            }
            else {
                reject(new Error("Недостаточно средств"));
            }
        }, 2000);
    });
};
const deliverOrder = () => {
    return new Promise((resolve) => {
        console.log("Доставка..");
        setTimeout(() => {
            resolve("Заказ доставлен.");
        }, 1500);
    });
};
checkStock("Пицца").then((order) => processPayment(order)).then(() => deliverOrder()).then((message) => console.log(message)).catch((error) => console.error(`Ошибка: ${error.message}`)).finally(() => { console.log("---"); console.log("Спасибо за заказ, приходите еще!"); });
