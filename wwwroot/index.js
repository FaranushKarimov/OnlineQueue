window.external.notify(`{"action":"get_categories"}`)
var modalContent = document.getElementById('modalContent')

let USERS = []
const PAG_COUNT = 6
let GLOBAL_TIMEOUT = null

function getUsers(categoryId) {
    window.external.notify(`{"action":"get_users","data":"${categoryId}"}`)
}


function clearAll() {
    let rootEl = document.getElementById('root')
    rootEl.innerHTML = ''
}

function disabled(id) {
    const next = document.getElementById(id)
    if (next) {
        next.classList.add('disabled')
    }
}

const openModal = (content) => {
    modalContent.innerHTML = ''
    modalContent.insertAdjacentHTML('beforeend', content);
    modal.classList.remove('hidden')
}

function closeModal() {
    let modal = document.getElementById('modal')
    clearAll()
    clearGlobalTimeOut()
    modal.classList.add('hidden')
    window.external.notify(`{"action":"get_categories"}`)
}


function notify(text) {
    const notify = document.getElementById('notify')
    const message = document.getElementById('message')
    message.innerHTML = text
    notify.classList.remove('hidden')

    setTimeout(() => {
        notify.classList.add('hidden')
    }, 5000)
}

function render(categories) {
    let rootEl = document.getElementById('root')

    clearAll()

    let parsedCategories = JSON.parse(categories)

    if (parsedCategories && parsedCategories.length) {
        rootEl.innerHTML = `
            <div class="flex justify-center">
                <div class="w-full md:w-1/2 lg:w-1/3">
                    ${parsedCategories.map(category => `
                        <button class="btn bg-blue-500 hover:bg-blue-600 text-white rounded-lg mb-8 h-28 w-full" onclick="getUsers(${category.id})">
                            ${category.name}
                        </button>
                    `)}
                </div>  
            </div>
        `
    } else {
        rootEl.innerHTML = `
            <div class="flex items-center justify-center"><h4>Категории пока отсутствуют</h4></div>
        `
    }
}

function showUsers(users) {
    let rootEl = document.getElementById('root')

    clearAll()

    let parsedUsers = JSON.parse(users)

    if (parsedUsers && parsedUsers.length) {
        // rootEl.innerHTML = `
        //     <div class="grid grid-cols-3 gap-4">
        //         ${parsedUsers.map(user => `
        //             <button class="btn bg-blue-500 hover:bg-blue-600 text-white rounded-lg mb-8 py-6 px-10" onclick="addToQueue(${user.id})">
        //                 ${user.full_name}
        //             </button>
        //         `)}
        //     </div>
        // `
        USERS = parsedUsers
        START()
    } else {
        rootEl.innerHTML = `
            <div class="flex items-center justify-center"><h4>Сотрудники пока отсутствуют</h4></div>
        `
    }
}

function addToQueue(id) {
    window.QUEUE_ID = id
    if (!window.TIKET_DISABLED) {
        window.TIKET_DISABLED = true
        window.external.notify(`{"action":"add_queue","data":"${id}"}`)
    }
}

function showTicket(ticket) {
    clearGlobalTimeOut()
    ticket = JSON.parse(ticket)
    window.TIKET_DISABLED = false
    const divElement =
        `<h2 class="text-xxl mb-2 pl-5 pt-5 text-center modal-title">Маълумоти пурра</h2>
        <div class="bg-white p-5 rounded-lg shadow-md mb-5 mx-auto">
            <div class="mb-8 pl-8">
                <div class="grid flex justify-between grid-cols-3 w-full py-2 border-b border-gray-200">
                    <span class="text-gray-500">Рақами чек</span>
                    <span>${ticket.id}</span>
                </div>
                <div class="grid flex justify-between grid-cols-3 w-full py-2 border-b border-gray-200">
                    <span class="text-gray-500">Рақами навбат</span>
                    <span>${ticket.number}</span>
                </div>
                <div class="grid flex justify-between grid-cols-3 w-full py-2 border-b border-gray-200">
                    <span class="text-gray-500">Самт</span>
                    <span>${ticket.category}</span>
                </div>
                <div class="grid flex justify-between grid-cols-3 w-full py-2 border-b border-gray-200">
                    <span class="text-gray-500">Ҳуҷра</span>
                    <span>${ticket.user}</span>
                </div>
                <div class="grid flex justify-between grid-cols-3 w-full py-2 border-b border-gray-200">
                    <span class="text-gray-500">Сана</span>
                    <span>${ticket.created_at}</span>
                </div>
            </div>
            <div class="flex items-center justify-center">
               <button type="button" class="h-24 bg-blue-500 hover:bg-blue-600 text-2xl text-white rounded-lg px-10 text-center justify-center items-center flex" type="button" onclick="closeModal()">
                   Ба асосӣ
               </button>
            </div>
        </div>`
    openModal(divElement)
    // return to home page after 5sec
    setGlobalTimeOut()
}

function setGlobalTimeOut() {
    GLOBAL_TIMEOUT = setTimeout(() => {
        closeModal()
    }, 7000)
}

function clearGlobalTimeOut() {
    clearTimeout(GLOBAL_TIMEOUT)
}

// start module users
function renderUsers(users) {
    clearGlobalTimeOut()
    let rootEl = document.getElementById('root')
    let usersStr = ''
    users.forEach(user => {
        usersStr += `<button class="user-button bg-blue-500 hover:bg-blue-600 text-white rounded-lg w-full" onclick="addToQueue(${user.id})"> ${user.full_name} </button>`
    })

    // last index
    const nextIndex = USERS.findIndex(el => el.id === users[users.length - 1].id)
    const prevIndex = USERS.findIndex(el => el.id === users[0].id)

    const paginationItem = `<div class="pagination-content">
    <button class="user-prev bg-blue-500 hover:bg-blue-600 text-white rounded-lg" onclick="prev(${prevIndex})"> Ба кафо </button>
    <button class="user-next bg-blue-500 hover:bg-blue-600 text-white rounded-lg" onclick="next(${nextIndex})"> Дигар нотариусҳо </button> </div>`

    const content = `<div class="users-content"> 
        <div>
        <div class="flex justify-start">
                   <button type="button" class="bg-blue-500 hover:bg-blue-600 home-button text-white rounded-lg" onclick="closeModal()">
                       Ба асосӣ
                   </button>
                </div> <div class="container mt-4"> ${usersStr} </div> ${paginationItem} </div>    
    </div>`

    rootEl.innerHTML = content
    setGlobalTimeOut()
}

function next(index) {
    if (index === USERS.length -1) {
        return
    }
    const users = []
    USERS.forEach((el, ind) => {
        if (ind > index && users.length !== PAG_COUNT) {
            users.push(el)
        }
    })
    renderUsers(users)
}


function prev(index) {
    if (index <= 0) {
        return
    }
    let users = []
    for (let i = index - 1; i !== -1; i--) {
        if (users.length !== PAG_COUNT) {
            const element = USERS[i]
            users.push(element)
        }
    }
    users = users.reverse()
    renderUsers(users)
}

function START() {
    const users = []

    USERS.forEach((el, ind) => {
        if (ind < PAG_COUNT) {
            users.push(el)
        }
    })

    renderUsers(users)
}
// end module users