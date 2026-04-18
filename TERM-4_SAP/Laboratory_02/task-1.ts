// 1
enum ProductCategory {
    NewArrival,    
    Discounted,    
    BestSeller,    
    Refurbished    
}

class Product {
    public readonly id: number;
    public name: string;
    public price: number;
    public description: string;
    public category: ProductCategory;

    constructor(product_id: number, product_name: string, product_price: number, product_description: string, product_category: ProductCategory) {
        this.id = product_id;
        this.name = product_name;
        this.price = product_price;
        this.description = product_description;
        this.category = product_category;
    }

    public getInfo(): void {
        console.log(`id: ${this.id}; name: ${this.name}; price: ${this.price}; description:  ${this.description}; category: ${ProductCategory[this.category]}`);
    }
}

function createProduct(data: Partial<Product>): Product {
    const id = data.id ?? 0;
    const name = data.name ?? "Нет названия";
    const price = data.price ?? 0;
    const description = data.description ?? "Нет описания";
    const category = data.category ?? 0;

    return new Product(id, name, price, description, category);
}

type addProductWithoutId = Omit<Product, 'id'>;

type OrderSummary = Pick<Order<Product>, 'id' | 'totalPrice'>;

//2
class Catalog {
    public products: Product[];

    constructor(catalog_products: Product[] = []) {
        this.products = catalog_products;
    }

    addProduct(new_product: Product): boolean {
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

    removeProduct(target_id: number): boolean {
        const original_lenght: number = this.products.length;
        this.products = this.products.filter(p => p.id !== target_id);
        const new_lenght: number = this.products.length;

        if (new_lenght < original_lenght) {
            console.log(`Продукт с id ${target_id} успешно удален`);
        }
        return new_lenght < original_lenght;
    }

    getProductById(target_id: number): boolean {
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

    getAllProducts(): void {
        for (const product of this.products) {
            product.getInfo();
        }
    }

    getProductsByCategory(target_category: ProductCategory): void {
        let isProducts: boolean = false;
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
class Order<T extends Product> {
    public id: number;
    public customer: Customer;
    public products: T[];
    public totalPrice: number;

    constructor(order_id: number, order_products: T[], custom: Customer) {
        this.id = order_id;
        this.customer = custom;
        this.products = order_products;
        this.totalPrice = 0;
        this.calculateTotalPrice();
    }

    private calculateTotalPrice() {
        this.totalPrice = this.products.reduce((sum, current) => sum + current.price, 0);
        console.log(`Рассчитана общая цена заказа: ${this.totalPrice}`);
    }

    public getOrderInfo() {
        console.log(`Заказ №${this.id}\nКоличество заказанных товаров: ${this.products.length}\nОбщая цена заказа: ${this.totalPrice}`);
    }
}

// 4
class Customer {
    public readonly id: number;
    public name: string;
    public email: string;

    constructor(customer_id: number, customer_name: string, customer_email: string) {
        this.id = customer_id;
        this.name = customer_name;
        this.email = customer_email;
    }

    public getCustomerInfo(): void {
        console.log(`Информация о заказчике:\nId:  ${this.id}\nИмя: ${this.name}\nПочта ${this.email}`);
    }
}

// 5 
class OrderManager {
    public orders: Array<Order<Product>>;

    constructor(new_orders: Array<Order<Product>> = []) {
        this.orders = new_orders;
    }

    public createOrder(order_id:number, products: Product[], customer: Customer): void {
        this.orders.push(new Order(order_id, products, customer));
        console.log("Заказ добавлен в менеджер");
    }

    public getOrderById(target_id: number): boolean {
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

    public getAllOrders(): void {
        for (const order of this.orders) {
            console.log(`Заказ найден.Информация о заказе:\nId заказчика: ${order.id}\nКоличество заказанных товаров: ${order.products.length}\nОбщая сумма заказа: ${order.totalPrice}`);
        }
    }

    public getOrdersByCustomer(target_customer: Customer): boolean{
        let orders = this.orders.filter(p =>p.customer === target_customer);
        if(orders.length > 0)
        {
            for(const order of orders)
            {
                console.log(`Заказ найден.Информация о заказе:\nId заказчика: ${order.id}\nКоличество заказанных товаров: ${order.products.length}\nОбщая сумма заказа: ${order.totalPrice}`);
            }
            return true;
        }
        else
        {
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

const summary: OrderSummary = {
    id: 1,
    totalPrice: 125000
};
console.log(`\nПример сводки (OrderSummary): Заказ №${summary.id}, Сумма: ${summary.totalPrice}`);