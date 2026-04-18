async function getData(): Promise<void> {
  try {
    const response = await fetch('https://jsonplaceholder.typicode.com/posts/1');

    if (!response.ok) {
      throw new Error(`Ошибка сети: ${response.status} ${response.statusText}`);
    }

    const data = await response.json();

    console.log("Данные получены:", data);

  } 
  catch (err) {
    console.error("Произошла ошибка при получении данных:", err);
  }
  finally {
    console.log("Запрос завершен.");
  }
}
getData();