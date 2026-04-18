var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
function limitRequests(urls, limit) {
    return __awaiter(this, void 0, void 0, function* () {
        const results = [];
        let currentIndex = 0;
        function executor() {
            return __awaiter(this, void 0, void 0, function* () {
                if (currentIndex >= urls.length) {
                    return;
                }
                const index = currentIndex++;
                const url = urls[index];
                if (!url) {
                    return;
                }
                try {
                    console.log(`Запуск: ${url} (Индекс: ${index})`);
                    const response = yield fetch(url);
                    if (!response.ok) {
                        throw new Error(`Ошибка HTTP: ${response.status}`);
                    }
                    results[index] = yield response.json();
                    console.log(`Готово: ${index}`);
                }
                catch (err) {
                    results[index] = err;
                    console.error(`Ошибка на индексе ${index}:`, err);
                }
                yield executor();
            });
        }
        const workers = [];
        const activeLimit = Math.min(limit, urls.length);
        for (let i = 0; i < activeLimit; i++) {
            workers.push(executor());
        }
        yield Promise.all(workers);
        return results;
    });
}
const images = Array.from({ length: 10 }, (_, i) => `https://jsonplaceholder.typicode.com/photos/${i + 1}`);
limitRequests(images, 3).then((data) => {
    console.log("Все запросы обработаны.");
    console.log("Результаты:", data);
});
