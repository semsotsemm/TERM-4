var _a, _b, _c;
// -------------------------------------
// Объекты на основе интерфейсов и типов
// -------------------------------------
function deepCopy(obj) {
    // Если это не объект или null, возвращаем как есть (примитивы)
    if (obj === null || typeof obj !== 'object') {
        return obj;
    }
    // Обработка массивов
    if (Array.isArray(obj)) {
        var copy_1 = [];
        for (var i = 0; i < obj.length; i++) {
            copy_1[i] = deepCopy(obj[i]);
        }
        return copy_1;
    }
    // Обработка объектов
    var copy = {};
    for (var key in obj) {
        if (Object.prototype.hasOwnProperty.call(obj, key)) {
            copy[key] = deepCopy(obj[key]);
        }
    }
    return copy;
}
var user = {
    name: 'Masha',
    age: 21
};
var numbers = [1, 2, 3];
var user1 = {
    name: 'Masha',
    age: 23,
    location: {
        city: 'Minsk',
        country: 'Belarus'
    }
};
var user2 = {
    name: 'Masha',
    age: 28,
    skills: ["HTML", "CSS", "JavaScript", "React"]
};
var array = [
    { id: 1, name: 'Vasya', group: 10 },
    { id: 2, name: 'Ivan', group: 11 },
    { id: 3, name: 'Masha', group: 12 },
    { id: 4, name: 'Petya', group: 10 },
    { id: 5, name: 'Kira', group: 11 },
];
var user4 = {
    name: 'Masha',
    age: 19,
    studies: {
        university: 'BSTU',
        speciality: 'designer',
        year: 2020,
        exams: {
            maths: true,
            programming: false
        }
    }
};
var user5 = {
    name: 'Masha',
    age: 22,
    studies: {
        university: 'BSTU',
        speciality: 'designer',
        year: 2020,
        department: {
            faculty: 'FIT',
            group: 10,
        },
        exams: [
            { maths: true, mark: 8 },
            { programming: true, mark: 4 },
        ]
    }
};
var user6 = {
    name: 'Masha',
    age: 21,
    studies: {
        university: 'BSTU',
        speciality: 'designer',
        year: 2020,
        department: {
            faculty: 'FIT',
            group: 10,
        },
        exams: [
            {
                maths: true,
                mark: 8,
                professor: {
                    name: 'Ivan Ivanov',
                    degree: 'PhD'
                }
            },
            {
                programming: true,
                mark: 10,
                professor: {
                    name: 'Petr Petrov',
                    degree: 'PhD'
                }
            },
        ]
    }
};
var user7 = {
    name: 'Masha',
    age: 20,
    studies: {
        university: 'BSTU',
        speciality: 'designer',
        year: 2020,
        department: {
            faculty: 'FIT',
            group: 10,
        },
        exams: [
            {
                maths: true,
                mark: 8,
                professor: {
                    name: 'Ivan Petrov',
                    degree: 'PhD',
                    articles: [
                        { title: "About HTML", pagesNumber: 3 },
                        { title: "About CSS", pagesNumber: 5 },
                        { title: "About JavaScript", pagesNumber: 1 },
                    ]
                }
            },
            {
                programming: true,
                mark: 10,
                professor: {
                    name: 'Petr Ivanov',
                    degree: 'PhD',
                    articles: [
                        { title: "About HTML", pagesNumber: 3 },
                        { title: "About CSS", pagesNumber: 5 },
                        { title: "About JavaScript", pagesNumber: 1 },
                    ]
                }
            },
        ]
    }
};
var store = {
    state: {
        profilePage: {
            post: [
                { id: 1, message: 'Hi', likeCount: 12 },
                { id: 2, message: 'By', likeCount: 1 },
            ],
            newPostText: "About me",
        },
        dialogsPage: {
            dialogs: [
                { id: 1, name: "Valera" },
                { id: 2, name: "Andrey" },
                { id: 3, name: "Sasha" },
                { id: 4, name: "Victor" },
            ],
            messages: [
                { id: 1, message: "hi" },
                { id: 2, message: "hi hi" },
                { id: 3, message: "hi hi hi" },
            ],
        },
        slidebar: [],
    },
};
// 1. Тест user7
var user7Copy = deepCopy(user7);
if (user7Copy.studies.exams[0]) {
    user7Copy.name = "Copy Masha";
    user7Copy.studies.exams[0].mark = 10;
    var firstArticle = user7Copy.studies.exams[0].professor.articles[0];
    if (firstArticle) {
        firstArticle.title = "HACKED TITLE";
    }
}
console.log("Тест User7:");
// Используем ?. для безопасного вывода
console.log("Оригинал (8):", (_a = user7.studies.exams[0]) === null || _a === void 0 ? void 0 : _a.mark);
console.log("Копия (10):", (_b = user7Copy.studies.exams[0]) === null || _b === void 0 ? void 0 : _b.mark);
// 2. Тест Store
var storeCopy = deepCopy(store);
storeCopy.state.profilePage.post.push({ id: 3, message: 'New Post', likeCount: 0 });
var firstMessage = storeCopy.state.dialogsPage.messages[0];
if (firstMessage) {
    firstMessage.message = "CHANGED MESSAGE";
}
console.log("\nТест Store:");
console.log("Сообщение в оригинале ('hi'):", (_c = store.state.dialogsPage.messages[0]) === null || _c === void 0 ? void 0 : _c.message);
