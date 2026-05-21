async function loadCelebrities() {
    const grid = document.getElementById('celebrity-grid');
    
    try {
        const response = await fetch('/api/Celebrities/data'); 
        const celebrities = await response.json();
        
        renderGrid(celebrities); 
    } catch (error) {
        console.error("Ошибка загрузки данных:", error);
    }
}

function renderGrid(data) {
    const grid = document.getElementById('celebrity-grid');
    grid.innerHTML = '';
    
    data.forEach(item => {
        const div = document.createElement('div');
        div.className = 'celeb-item';
        
        const id = item.id || item.Id;
        const name = item.fullName || item.FullName;
        const fileName = item.reqPhotoPath || item.ReqPhotoPath;

        const photoUrl = `/api/Celebrities/photo/${fileName}`;
        
        div.innerHTML = `<img src="${photoUrl}" alt="${name}" title="${name}">`;
        
        div.addEventListener('click', () => loadLifeEvents(id, name));
        grid.appendChild(div);
    });
}


async function loadLifeEvents(id, name) {
    const infoPanel = document.getElementById('info-panel');
    try {
        const response = await fetch(`/api/Liveevents/Celebrities/${id}`);
        const events = await response.json();
        
        displayInfo(name, events);
    } catch (error) {
        infoPanel.innerHTML = 'Ошибка при загрузке событий';
    }
}

function displayInfo(name, events) {
    const infoPanel = document.getElementById('info-panel');
    infoPanel.innerHTML = '';
    
    events.forEach(ev => {
        const line = document.createElement('div');
        line.className = 'info-line';
        
        const dateStr = ev.date || ev.Date || "";
        const desc = ev.description || ev.Description || "";

        line.innerHTML = `
            <span style="width: 200px; font-weight: bold;">${name}</span>
            <span style="width: 250px; color: gray;">${dateStr}</span>
            <span>${desc}</span>
        `;
        infoPanel.appendChild(line);
    });
}

document.addEventListener('DOMContentLoaded', loadCelebrities);