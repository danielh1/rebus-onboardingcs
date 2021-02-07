const createLi = (user) => {
    const li = document.createElement('li');
    // add user details to `li`
    li.textContent = `${user.id}: ${user.first_name} ${user.last_name}`;

    // attach onclick event
    li.onclick = e => deleteUser(li, user.id);

    return li;
};

const appendToDOM = (users) => {
    const ul = document.querySelector('ul');
    //iterate over all users
    users.map(user => {
        ul.appendChild(createLi(user));
    });
};

const fetchUsers = () => {
    axios.get('https://reqres.in/api/users')
        .then(response => {
            const users = response.data.data;
            console.log(`GET list users`, users);
            // append to DOM
            appendToDOM(users);
        })
        .catch(error => console.error(error));
};

// fetchUsers();

// register a new account
const createAccount = (first_name, last_name) => {
    axios.post('https://localhost:5100/newcustomer', null, {params: { name:first_name, email:last_name}})
        .then(response => {
            const addedUser = response;
            console.log(`POST: Account is added`, response.status);
            // append to DOM
            const account = { first_name, last_name };
            appendToDOM([account]);
        })
        .catch(error => console.error(error));
};




// event listener for form submission
const form = document.querySelector('form');

const formEvent = form.addEventListener('submit', event => {
    event.preventDefault();

    const first_name = document.querySelector('#first_name').value;
    const last_name = document.querySelector('#last_name').value;

    const user = { first_name, last_name };
    createAccount(first_name, last_name);
});


// delete a user
const deleteUser = (elem, id) => {
    axios.delete(`https://reqres.in/api/users/${id}`)
        .then(response => {
            console.log(`DELETE: user is removed`, id);
            // remove elem from DOM
            elem.remove();
        })
        .catch(error => console.error(error));
};

