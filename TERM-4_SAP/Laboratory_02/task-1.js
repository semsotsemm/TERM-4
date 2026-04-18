// 1
var ProductCategory;
(function (ProductCategory) {
    ProductCategory[ProductCategory["NewArrival"] = 0] = "NewArrival";
    ProductCategory[ProductCategory["Discounted"] = 1] = "Discounted";
    ProductCategory[ProductCategory["BestSeller"] = 2] = "BestSeller";
    ProductCategory[ProductCategory["Refurbished"] = 3] = "Refurbished";
})(ProductCategory || (ProductCategory = {}));
class Product {
    constructor(product_id, product_name, product_price, product_description, product_category) {
        this.id = product_id;
        this.name = product_name;
        this.price = product_price;
        this.description = product_description;
        this.category = product_category;
    }
    getInfo() {
        console.log(`id: ${this.id}; name: ${this.name}; price: ${this.price}; description:  ${this.description}; category: ${ProductCategory[this.category]}`);
    }
}
function createProduct(data) {
    var _a, _b, _c, _d, _e;
    const id = (_a = data.id) !== null && _a !== void 0 ? _a : 0;
    const name = (_b = data.name) !== null && _b !== void 0 ? _b : "Нет названия";
    const price = (_c = data.price) !== null && _c !== void 0 ? _c : 0;
    const description = (_d = data.description) !== null && _d !== void 0 ? _d : "Нет описания";
    const category = (_e = data.category) !== null && _e !== void 0 ? _e : 0;
    return new Product(id, name, price, description, category);
}
//2
class Catalog {
    constructor(catalog_products = []) {
        this.products = catalog_products;
    }
    addProduct(new_product) {
        for (const product of this.products) {
            if (product.id == new_product.id) {
                console.log(`Не удалось добавить новый продукт. Продукт с id ${new_product.id} уже существует в каталоге `);
                return false;
            }
        }
        this.products.push(new_product);
        console.log(`Новый продукт успешно добавлен в каталог, теперь можно обратится к нему по ${new_product.id}`);
        return true;
    }
    removeProduct(target_id) {
        const original_lenght = this.products.length;
        this.products = this.products.filter(p => p.id !== target_id);
        const new_lenght = this.products.length;
        if (new_lenght < original_lenght) {
            console.log(`Продукт с id ${target_id} успешно удален`);
        }
        return new_lenght < original_lenght;
    }
    getProductById(target_id) {
        const product = this.products.find(p => p.id === target_id);
        if (product) {
            console.log(`Продукт с id ${target_id} успешно найден. Информация о продукте:`);
            product.getInfo();
            return true;
        }
        else {
            console.log(`Продукт с id ${target_id} нет в каталоге.`);
            return false;
        }
    }
    getAllProducts() {
        for (const product of this.products) {
            product.getInfo();
        }
    }
    getProductsByCategory(target_category) {
        let isProducts = false;
        for (const product of this.products) {
            if (product.category === target_category) {
                isProducts = true;
                product.getInfo();
            }
        }
        if (!isProducts) {
            console.log(`Список продуктов с категорией ${target_category} пуст.`);
        }
    }
}
// 3
class Order {
    constructor(order_id, order_products, custom) {
        this.id = order_id;
        this.customer = custom;
        this.products = order_products;
        this.totalPrice = 0;
        this.calculateTotalPrice();
    }
    calculateTotalPrice() {
        this.totalPrice = this.products.reduce((sum, current) => sum + current.price, 0);
        console.log(`Рассчитана общая цена заказа: ${this.totalPrice}`);
    }
    getOrderInfo() {
        console.log(`Заказ №${this.id}\nКоличество заказанных товаров: ${this.products.length}\nОбщая цена заказа: ${this.totalPrice}`);
    }
}
// 4
class Customer {
    constructor(customer_id, customer_name, customer_email) {
        this.id = customer_id;
        this.name = customer_name;
        this.email = customer_email;
    }
    getCustomerInfo() {
        console.log(`Информация о заказчике:\nId:  ${this.id}\nИмя: ${this.name}\nПочта ${this.email}`);
    }
}
// 5 
class OrderManager {
    constructor(new_orders = []) {
        this.orders = new_orders;
    }
    createOrder(order_id, products, customer) {
        this.orders.push(new Order(order_id, products, customer));
        console.log("Заказ добавлен в менеджер");
    }
    getOrderById(target_id) {
        const order = this.orders.find(p => p.id === target_id);
        if (order) {
            console.log(`Заказ найден.Информация о заказе:\nId заказчика: ${order.id}\nКоличество заказанных товаров: ${order.products.length}\nОбщая сумма заказа: ${order.totalPrice}`);
            return true;
        }
        else {
            console.log(`Заказа с id ${target_id} не существует в менеджере.`);
            return false;
        }
    }
    getAllOrders() {
        for (const order of this.orders) {
            console.log(`Заказ найден.Информация о заказе:\nId заказчика: ${order.id}\nКоличество заказанных товаров: ${order.products.length}\nОбщая сумма заказа: ${order.totalPrice}`);
        }
    }
    getOrdersByCustomer(target_customer) {
        let orders = this.orders.filter(p => p.customer === target_customer);
        if (orders.length > 0) {
            for (const order of orders) {
                console.log(`Заказ найден.Информация о заказе:\nId заказчика: ${order.id}\nКоличество заказанных товаров: ${order.products.length}\nОбщая сумма заказа: ${order.totalPrice}`);
            }
            return true;
        }
        else {
            console.log(`Заказчик ${target_customer} отсутствует в менеджере`);
            return false;
        }
    }
}
const myCatalog = new Catalog();
const laptop = new Product(101, "Игровой ноутбук", 120000, "RTX 4090, 32GB RAM", ProductCategory.BestSeller);
const mouse = new Product(102, "Беспроводная мышь", 5000, "Оптическая, 16000 DPI", ProductCategory.NewArrival);
const keyboard = createProduct({ id: 103, name: "Механическая клавиатура", price: 8000 });
myCatalog.addProduct(laptop);
myCatalog.addProduct(mouse);
myCatalog.addProduct(keyboard);
console.log("\n--- Весь каталог товаров ---");
myCatalog.getAllProducts();
const ivan = new Customer(1, "Иван Иванов", "ivan@example.com");
ivan.getCustomerInfo();
const myManager = new OrderManager();
console.log("\n--- Создание заказов ---");
myManager.createOrder(1, [laptop, mouse], ivan);
myManager.createOrder(2, [keyboard], ivan);
console.log("\n--- Проверка поиска по ID заказа ---");
myManager.getOrderById(1);
console.log("\n--- Проверка всех заказов конкретного клиента ---");
myManager.getOrdersByCustomer(ivan);
console.log("\n--- Проверка фильтрации каталога по категории ---");
myCatalog.getProductsByCategory(ProductCategory.NewArrival);
const summary = {
    id: 1,
    totalPrice: 125000
};
console.log(`\nПример сводки (OrderSummary): Заказ №${summary.id}, Сумма: ${summary.totalPrice}`);
