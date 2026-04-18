interface myUser {
    name:string;
    age: number;
}

type myNumbers = number[];

interface myUser1 extends myUser{
    location: myLocation;
}

interface myLocation{
    city: string;
    country: string;
}

interface myUser2 extends myUser {
    skills: mySkills;
}

type mySkills = string[];

type myArray = myArrayElements[];

interface myArrayElements{
    id: number;
    name: string;
    group: number;
}

interface myUser4 extends myUser{
    studies: myStadies;
}

interface myStadies{
    university: string;
    speciality: string;
    year: number;
    exams:myStadiesExams;
}

interface myStadiesExams{
    [key:string]: boolean;
}

interface myUser5 extends myUser{
    studies:myStadies5;
}

type faculty2 = {
    id: number;
}



interface myStadies5{
    university: string;
    speciality: string;
    year: number;
    department: myDepartment5;
    exams:myStadiesExams5[];
}

interface myStadiesExams5{
    [key:string]: boolean | number;
    mark: number;
}

interface myDepartment5{
    faculty: string;
    group: number;
}

interface myUser6 extends myUser{
    studies: myStadies6;
}

interface myStadies6{
    university: string;
    speciality: string;
    year: number;
    department: myDepartment5;
    exams:myStadiesExams6[];
}

interface myStadiesExams6{
    [key: string]:any;
    mark:number;
    professor: myProfessor;
}

interface myProfessor{
    name: string;
    degree:string;
}

interface myUser7 extends myUser{
    studies:myStadies7;
}

interface myStadies7{
    university: string;
    speciality:string;
    year: number;
    department: myDepartment5;
    exams:myStadiesExams7[];
}

interface myStadiesExams7{
    [key:string]: any;
    mark:number;
    professor:myProfessor7;
}

interface myProfessor7{
    name: string;
    degree: string;
    articles:myArticles[];
}

interface myArticles{
    title: string;
    pagesNumber: number;
}

interface myStore{
    state:myState;
}

interface myState{
    profilePage: myProfilePage;
    dialogsPage: myDailogsPage;
    slidebar: any[];
}

interface myProfilePage{
    post: myPost[];
    newPostText: string;
}

interface myPost{
    id:number;
    message:string;
    likeCount: number;
}

interface myDailogsPage{
    dialogs:myDailogs[];
    messages:myMessages[];
}

interface myDailogs{
    id:number;
    name:string;
}

interface myMessages{
    id:number;
    message:string;
}

// -------------------------------------
// Объекты на основе интерфейсов и типов
// -------------------------------------
function deepCopy<T>(obj: T): T {
    if (obj === null || typeof obj !== 'object') {
        return obj;
    }

    if (Array.isArray(obj)) {
        const copy = [] as any[];
        for (let i = 0; i < obj.length; i++) {
            copy[i] = deepCopy(obj[i]);
        }
        return copy as any as T;
    }

    const copy = {} as T;
    for (const key in obj) {
        if (Object.prototype.hasOwnProperty.call(obj, key)) {
            copy[key] = deepCopy(obj[key]);
        }
    }
    return copy;
}

let user:myUser = {
    name: 'Masha',
    age: 21
};

let numbers:myNumbers = [1, 2, 3];

let user1: myUser1= {
    name: 'Masha',
    age: 23,
    location: {
        city: 'Minsk',
        country: 'Belarus'
    }
};

let user2: myUser2 = {
    name: 'Masha',
    age: 28,
    skills: ["HTML", "CSS", "JavaScript", "React"]
};

const array:myArray = [
    {id: 1, name: 'Vasya', group: 10}, 
    {id: 2, name: 'Ivan', group: 11},
    {id: 3, name: 'Masha', group: 12},
    {id: 4, name: 'Petya', group: 10},
    {id: 5, name: 'Kira', group: 11},
];

let user4:myUser4 = {
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

let user5:myUser5 = {
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
            { maths: true, mark: 8},
            { programming: true, mark: 4},
        ]
    }
};

let user6:myUser6 = {
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

let user7:myUser7 = {
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
                        {title: "About HTML", pagesNumber: 3},
                        {title: "About CSS", pagesNumber: 5},
                        {title: "About JavaScript", pagesNumber: 1},
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
                        {title: "About HTML", pagesNumber: 3},
                        {title: "About CSS", pagesNumber: 5},
                        {title: "About JavaScript", pagesNumber: 1},
                    ]
                }
            },
        ]
    }
};

let store: myStore= {
    state:{
        profilePage:{
            post:[
                {id:1, message:'Hi', likeCount:12},
                {id:2, message:'By', likeCount:1},
            ],
            newPostText:"About me",
        },
        dialogsPage:{
            dialogs:[
                {id:1, name:"Valera"},
                {id:2, name:"Andrey"},
                {id:3, name:"Sasha"},
                {id:4, name:"Victor"},
            ],
            messages:[
                {id:1, message:"hi"},
                {id:2, message:"hi hi"},
                {id:3, message:"hi hi hi"},
            ],
        },
        slidebar:[],
    },
};

const user7Copy = deepCopy(user7);

if (user7Copy.studies.exams[0]) {
    user7Copy.name = "Copy Masha";
    user7Copy.studies.exams[0].mark = 10;
    
    const firstArticle = user7Copy.studies.exams[0].professor.articles[0];
    if (firstArticle) {
        firstArticle.title = "HACKED TITLE";
    }
}

console.log("Тест User7:");
console.log("Оригинал (8):", user7.studies.exams[0]?.mark);
console.log("Копия (10):", user7Copy.studies.exams[0]?.mark);

const storeCopy = deepCopy(store);

storeCopy.state.profilePage.post.push({ id: 3, message: 'New Post', likeCount: 0 });

const firstMessage = storeCopy.state.dialogsPage.messages[0];
if (firstMessage) {
    firstMessage.message = "CHANGED MESSAGE";
}

console.log("\nТест Store:");
console.log("Сообщение в оригинале ('hi'):", store.state.dialogsPage.messages[0]?.message);