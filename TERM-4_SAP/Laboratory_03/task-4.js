console.log('Начало');
setTimeout(function () { return console.log('Таймаут'); }, 0);
Promise.resolve()
    .then(function () { return console.log('Промис 1'); })
    .then(function () { return console.log('Промис 2'); });
console.log('Конец');
