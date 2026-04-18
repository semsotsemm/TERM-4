const fastFetch = (): Promise<string> => {
    return new Promise((resolve) => {
        setTimeout(() => { resolve("фастФеч") }, 2000)
    })
}

const slowFetch = (): Promise<string> => {
    return new Promise((resolve) => {
        setTimeout(() => {
            resolve("слоуФеч")
        }, 2000);
    })
}

Promise.race([fastFetch(), slowFetch()]).then((winner) => {
    console.log(winner)
}).catch((error) => {
    console.error(error)
})