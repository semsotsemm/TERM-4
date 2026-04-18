async function limitRequests(urls: string[], limit: number): Promise<any[]> {
    const results: any[] = [];
    let currentIndex = 0;

    async function executor(): Promise<void> {
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
            const response = await fetch(url);

            if (!response.ok) {
                throw new Error(`Ошибка HTTP: ${response.status}`);
            }

            results[index] = await response.json();
            console.log(`Готово: ${index}`);
        } catch (err) {
            results[index] = err;
            console.error(`Ошибка на индексе ${index}:`, err);
        }

        await executor();
    }

    const workers: Promise<void>[] = [];

    const activeLimit = Math.min(limit, urls.length);
    for (let i = 0; i < activeLimit; i++) {
        workers.push(executor());
    }

    await Promise.all(workers);
    return results;
}

const images = Array.from({ length: 10 }, (_, i) =>
    `https://jsonplaceholder.typicode.com/photos/${i + 1}`
);

limitRequests(images, 3).then((data) => {
    console.log("Все запросы обработаны.");
    console.log("Результаты:", data);
});