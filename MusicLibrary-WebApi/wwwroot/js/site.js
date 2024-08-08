document.addEventListener('DOMContentLoaded', function () {

    // Sections rendering and elements visibility:
    const elementsToHide = [
        '#link-users',
        '#link-genres',
        '#link-tunes',
        '#greeting',
        '#link-logout',
        '#link-back',
        '#tunes-filter',
        '#tunes-container',
        '#users-filter',
        '#users-container',
        '#users-edit-container',
        '#tunes-create-container',
        '#tunes-edit-container'
    ];

    const elementsToShow = [
        '#tunes-filter',
        '#tunes-container',
        '#link-users',
        '#link-genres',
        '#link-tunes',
        '#greeting',
        '#link-logout'
    ];

    const commonElementsToShow = [
        '#link-users',
        '#link-genres',
        '#link-tunes',
        '#greeting',
        '#link-logout'
    ];

    function updateVisibility(isLoggedIn) {
        elementsToHide.forEach(selector => {
            const element = document.querySelector(selector);
            if (element) {
                element.style.display = 'none';
            }
        });

        elementsToShow.forEach(selector => {
            const element = document.querySelector(selector);
            if (element) {
                element.style.display = isLoggedIn ? '' : 'none';
            }
        });

        document.getElementById('login-container').style.display = isLoggedIn ? 'none' : '';
    }

    const user = sessionStorage.getItem('user');
    const isLoggedIn = user !== null;
    updateVisibility(isLoggedIn);

    const linkUsers = document.getElementById('link-users');
    const usersFilter = document.getElementById('users-filter');
    const usersContainer = document.getElementById('users-container');

    linkUsers.addEventListener('click', function (event) {
        event.preventDefault();

        elementsToHide.forEach(selector => {
            const element = document.querySelector(selector);
            if (element) {
                element.style.display = 'none';
            }
        });

        commonElementsToShow.forEach(selector => {
            const element = document.querySelector(selector);
            if (element) {
                element.style.display = '';
            }
        });

        usersFilter.style.display = '';
        usersContainer.style.display = '';
    });

    const linkGenres = document.getElementById('link-genres');
    const genresFilter = document.getElementById('genres-filter');
    const genresContainer = document.getElementById('genres-container');

    linkGenres.addEventListener('click', function (event) {
        event.preventDefault();

        elementsToHide.forEach(selector => {
            const element = document.querySelector(selector);
            if (element) {
                element.style.display = 'none';
            }
        });

        commonElementsToShow.forEach(selector => {
            const element = document.querySelector(selector);
            if (element) {
                element.style.display = '';
            }
        });

        genresFilter.style.display = '';
        genresContainer.style.display = '';
    });

    const tunesLink = document.getElementById('link-tunes');
    const tunesFilter = document.getElementById('tunes-filter');
    const tunesContainer = document.getElementById('tunes-container');

    tunesLink.addEventListener('click', function (event) {
        event.preventDefault();

        elementsToHide.forEach(selector => {
            const element = document.querySelector(selector);
            if (element) {
                element.style.display = 'none';
            }
        });

        commonElementsToShow.forEach(selector => {
            const element = document.querySelector(selector);
            if (element) {
                element.style.display = '';
            }
        });

        tunesFilter.style.display = '';
        tunesContainer.style.display = '';
    });

    const createTuneLink = document.getElementById('link-create-tune');
    const tunesCreateContainer = document.getElementById('tunes-create-container');
    const usersEditContainer = document.getElementById('users-edit-container');
    const backLink = document.getElementById('link-back');

    createTuneLink.addEventListener('click', function (event) {
        event.preventDefault();

        tunesFilter.style.display = 'none';
        tunesContainer.style.display = 'none';
        logoutLink.style.display = 'none';

        backLink.style.display = '';
        tunesCreateContainer.style.display = '';
    });

    function homePageView() {
        backLink.style.display = 'none';
        tunesCreateContainer.style.display = 'none';
        tunesEditContainer.style.display = 'none';
        usersEditContainer.style.display = 'none';

        tunesFilter.style.display = '';
        tunesContainer.style.display = '';
        logoutLink.style.display = '';
    }

    backLink.addEventListener('click', function (event) {
        event.preventDefault();
        homePageView();
    });

    // 'Tunes General Section' rendering and 'TuneController GET action' logic:
    const genreSelect = document.getElementById('genre-select');
    const tuneList = document.getElementById('tune-list');
    const prevPageLink = document.getElementById('prev-page');
    const nextPageLink = document.getElementById('next-page');
    const pageSize = 5;

    let currentPage = 1;
    let selectedGenre = 0;

    function loadGenres() {
        fetch('/api/category')
            .then(response => response.json())
            .then(genres => {
                genreSelect.innerHTML = '';

                const allGenresOption = document.createElement('option');
                allGenresOption.value = '';
                allGenresOption.textContent = 'All Genres';
                genreSelect.appendChild(allGenresOption);

                const newOption = document.createElement('option');
                newOption.value = 'new';
                newOption.textContent = 'New';
                genreSelect.appendChild(newOption);

                const blockedOption = document.createElement('option');
                blockedOption.value = 'blocked';
                blockedOption.textContent = 'Blocked';
                genreSelect.appendChild(blockedOption);

                genres.forEach(genre => {
                    const option = document.createElement('option');
                    option.value = genre.id;
                    option.textContent = genre.genre;
                    genreSelect.appendChild(option);
                });
            })
            .catch(error => {
                console.error('Error loading genres:', error);
            });
    }

    function loadTunes() {
        fetch(`/api/tune?selected=${selectedGenre}&pageNumber=${currentPage}&pageSize=${pageSize}`)
            .then(response => response.json())
            .then(data => {
                tuneList.innerHTML = '';
                data.tunes.forEach(tune => {
                    const tuneElement = document.createElement('div');
                    tuneElement.classList.add('tune-box');
                    tuneElement.innerHTML =
                    `<div class="tune-item-image">
                        <img src="${tune.posterUrl}" alt="${tune.title}" />
                    </div>
                    <div class="tune-box-content">
                        <div class="tune-item-description">
                            <a class="link_type_d link-edit-tune" href="#" data-tune-id="${tune.id}">${tune.performer} - ${tune.title}</a>
                        </div>
                        <div class="tune-item-audio">
                            <audio controls>
                                <source src="${tune.fileUrl}" type="audio/mpeg">
                            </audio>
                        </div>
                    </div>`;
                    tuneList.appendChild(tuneElement);
                });

                prevPageLink.style.display = data.pagination.hasPreviousPage ? '' : 'none';
                nextPageLink.style.display = data.pagination.hasNextPage ? '' : 'none';
            });
    }

    function loadPreviousPage() {
        if (currentPage > 1) {
            currentPage--;
            loadTunes();
        }
    }

    function loadNextPage() {
        currentPage++;
        loadTunes();
    }

    genreSelect.addEventListener('change', function () {
        selectedGenre = parseInt(this.value, 10);
        currentPage = 1;
        loadTunes();
    });

    prevPageLink.addEventListener('click', function (event) {
        event.preventDefault();
        loadPreviousPage();
    });

    nextPageLink.addEventListener('click', function (event) {
        event.preventDefault();
        loadNextPage();
    });

    loadGenres();
    loadTunes();

    // 'Tunes Create Section' rendering and 'TuneController POST' actiong logic:

    const tunesCreateForm = document.querySelector('#tunes-create-container .user-create-form');
    const performerInput = document.getElementById('performer');
    const titleInput = document.getElementById('title');
    const categoryIdSelect = document.getElementById('category-id');
    const fileInput = document.getElementById('file');
    const posterInput = document.getElementById('poster');

    tunesCreateForm.addEventListener('submit', function (event) {
        event.preventDefault();

        let isValid = true;

        document.querySelectorAll('.validation-message').forEach(el => el.textContent = '');

        if (!performerInput.value.trim()) {
            document.getElementById('performer-error').textContent = 'Artist Name is required.';
            isValid = false;
        }

        if (!titleInput.value.trim()) {
            document.getElementById('title-error').textContent = 'Title is required.';
            isValid = false;
        }

        if (categoryIdSelect.value === "") {
            document.getElementById('category-error').textContent = 'Please select a category.';
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        const formData = new FormData();
        formData.append('performer', performerInput.value.trim());
        formData.append('title', titleInput.value.trim());
        formData.append('categoryId', parseInt(categoryIdSelect.value));
        formData.append('file', fileInput.files[0]);
        formData.append('poster', posterInput.files[0]);

        fetch('/api/tune', {
            method: 'POST',
            body: formData
        }).then(tune => {
            loadTunes();
            tunesCreateContainer.style.display = 'none';
            backLink.style.display = 'none';
            tunesFilter.style.display = '';
            tunesContainer.style.display = '';
            logoutLink.style.display = '';
            alert('Tune successfully created!');
        });
    });

    // 'Tunes Update Section' rendering and 'TuneController PUT' action logic:
    document.addEventListener('click', function (event) {
        const target = event.target;
        if (target.classList.contains('link-edit-tune')) {
            event.preventDefault();
            const tuneId = target.getAttribute('data-tune-id');
            fetch(`/api/tune/${tuneId}`)
                .then(response => response.json())
                .then(tune => {
                    document.getElementById('tune-id').value = tune.id;
                    document.getElementById('edit-performer').value = tune.performer;
                    document.getElementById('edit-title').value = tune.title;
                    document.getElementById('is-authorize').value = tune.isAuthorized ? '1' : '0';
                    document.getElementById('is-blocked').value = tune.isBlocked ? '1' : '0';
                    document.getElementById('edit-category-id').value = tune.categoryId;

                    tunesFilter.style.display = 'none';
                    tunesContainer.style.display = 'none';
                    logoutLink.style.display = 'none';
                    tunesEditContainer.style.display = '';
                    backLink.style.display = '';
                });
        }
    });

    function loadCategories(selectElementId) {
        fetch('/api/category')
            .then(response => response.json())
            .then(categories => {
                const selectElement = document.getElementById(selectElementId);
                selectElement.innerHTML = '';

                const placeholderOption = document.createElement('option');
                placeholderOption.value = '';
                placeholderOption.textContent = 'Select a Category';
                selectElement.appendChild(placeholderOption);

                categories.forEach(category => {
                    const option = document.createElement('option');
                    option.value = category.id;
                    option.textContent = category.genre;
                    selectElement.appendChild(option);
                });
            });
    }
    loadCategories('category-id');
    loadCategories('edit-category-id');
    function loadAuthorizeOptions() {
        const authorizeSelect = document.getElementById('is-authorize');
        authorizeSelect.innerHTML = '';

        const authorizedOption = document.createElement('option');
        authorizedOption.value = '0';
        authorizedOption.textContent = 'Authorized';
        authorizeSelect.appendChild(authorizedOption);

        const unauthorizedOption = document.createElement('option');
        unauthorizedOption.value = '1';
        unauthorizedOption.textContent = 'Unauthorized';
        authorizeSelect.appendChild(unauthorizedOption);
    }
    loadAuthorizeOptions();
    function loadBlockedOptions() {
        const blockedSelect = document.getElementById('is-blocked');
        blockedSelect.innerHTML = '';

        const blockedOption = document.createElement('option');
        blockedOption.value = '0';
        blockedOption.textContent = 'Blocked';
        blockedSelect.appendChild(blockedOption);

        const unblockedOption = document.createElement('option');
        unblockedOption.value = '1';
        unblockedOption.textContent = 'Unblocked';
        blockedSelect.appendChild(unblockedOption);
    }
    loadBlockedOptions();

    const tunesEditContainer = document.getElementById('tunes-edit-container');
    const tunesEditForm = document.querySelector('#tunes-edit-container .user-create-form');

    tunesEditForm.addEventListener('submit', function (event) {
        event.preventDefault();

        let isValid = true;

        document.querySelectorAll('.validation-message').forEach(el => el.textContent = '');

        const performer = document.getElementById('edit-performer').value.trim();
        const title = document.getElementById('edit-title').value.trim();
        const categoryId = document.getElementById('edit-category-id').value;

        if (!performer) {
            document.getElementById('edit-performer-error').textContent = 'Artist Name is required.';
            isValid = false;
        }

        if (!title) {
            document.getElementById('edit-title-error').textContent = 'Title is required.';
            isValid = false;
        }

        if (categoryId === "") {
            document.getElementById('edit-category-error').textContent = 'Please select a category.';
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        const tuneId = document.getElementById('tune-id').value;
        const formData = new FormData(tunesEditForm);

        fetch(`/api/tune/${tuneId}`, {
            method: 'PUT',
            body: formData
        }).then(updatedTune => {
            loadTunes();
            tunesEditContainer.style.display = 'none';
            backLink.style.display = 'none';
            tunesFilter.style.display = '';
            tunesContainer.style.display = '';
            logoutLink.style.display = '';
            alert('Tune updated successfully!');
            updateVisibility(true);
        });
    });

    // 'TuneController DELETE' action logic:
    document.getElementById('delete-tune').addEventListener('click', function (e) {
        e.preventDefault();
        const tuneId = document.getElementById('tune-id').value;

        if (confirm('Are you sure you want to delete this tune?')) {
            fetch(`/api/tune / ${tuneId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            }).then(response => {
                if (response.ok) {
                    alert('Tune deleted successfully.');
                    updateVisibility(true);
                } else {
                    alert('Error deleting tune.');
                }
            });
        }
    });

    // 'Users General Section' rendering and 'UserController GET' action logic:
    const userSelect = document.getElementById('user-select');
    const prevUserPageLink = document.getElementById('prev-user-page');
    const nextUserPageLink = document.getElementById('next-user-page');
    const userList = document.getElementById('user-list');
    const userPageSize = 10;

    let currentUserPage = 1;
    let selectedUserFilter = 0;

    function loadUsers() {
        fetch(`/api/user?pageNumber=${currentUserPage}&pageSize=${userPageSize}&selected=${selectedUserFilter}`)
            .then(response => response.json())
            .then(users => {
                userList.innerHTML = users.length ?
                    `<div class="content-box">
                        ${users.map(user => `<a class="item" href="#" data-id="${user.id}">${user.username}</a>`).join('')}
                    </div>`
                    : '<h2 class="not_found">No users found</h2>';

                prevUserPageLink.style.display = currentUserPage > 1 ? '' : 'none';
                nextUserPageLink.style.display = users.length === pageSize ? '' : 'none';
            })
    }

    function loadPreviousUserPage() {
        if (currentUserPage > 1) {
            currentUserPage--;
            loadUsers();
        }
    }

    function loadNextUserPage() {
        currentUserPage++;
        loadUsers();
    }

    userSelect.addEventListener('change', function () {
        selectedUserFilter = parseInt(this.value, 10);
        currentUserPage = 1;
        loadUsers();
    });

    prevUserPageLink.addEventListener('click', function (event) {
        event.preventDefault();
        loadPreviousUserPage();
    });

    nextUserPageLink.addEventListener('click', function (event) {
        event.preventDefault();
        loadNextUserPage();
    });

    loadUsers();

    // 'Users Login Section' rendering and 'UserController POST' action logic:
    const loginForm = document.getElementById('login-form');
    const usernameInput = document.getElementById('username');
    const passwordInput = document.getElementById('password');
    const usernameError = document.getElementById('username-error');
    const passwordError = document.getElementById('password-error');

    loginForm.addEventListener('submit', function (event) {
        event.preventDefault();

        usernameError.textContent = '';
        passwordError.textContent = '';

        const username = usernameInput.value.trim();
        const password = passwordInput.value.trim();

        let isValid = true;

        if (username === '') {
            usernameError.textContent = 'Username is required.';
            isValid = false;
        }

        if (password === '') {
            passwordError.textContent = 'Password is required.';
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        fetch('/api/user/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Login failed');
                }
                return response.json();
            })
            .then(user => {
                sessionStorage.setItem('user', JSON.stringify(user));
                document.getElementById('greeting').textContent = `Hello, ${user.username}`;
                updateVisibility(true);
            })
            .catch(error => {
                passwordError.textContent = 'Invalid username or password.';
            });
    });

    const logoutLink = document.getElementById('link-logout');
    logoutLink.addEventListener('click', function (event) {
        event.preventDefault();
        sessionStorage.removeItem('user');
        updateVisibility(false);
    });

    // 'Users Edit Section' rendering and 'UserController PUT' action logic:
    function loadUserAuthorizeOptions() {
        const authorizeSelect = document.getElementById('is-user-authorized');
        authorizeSelect.innerHTML = '';

        const authorizedOption = document.createElement('option');
        authorizedOption.value = '0';
        authorizedOption.textContent = 'Authorized';
        authorizeSelect.appendChild(authorizedOption);

        const unauthorizedOption = document.createElement('option');
        unauthorizedOption.value = '1';
        unauthorizedOption.textContent = 'Unauthorized';
        authorizeSelect.appendChild(unauthorizedOption);
    }
    loadUserAuthorizeOptions();

    function loadUserBlockedOptions() {
        const blockedSelect = document.getElementById('is-user-blocked');
        blockedSelect.innerHTML = '';

        const blockedOption = document.createElement('option');
        blockedOption.value = '0';
        blockedOption.textContent = 'Blocked';
        blockedSelect.appendChild(blockedOption);

        const unblockedOption = document.createElement('option');
        unblockedOption.value = '1';
        unblockedOption.textContent = 'Unblocked';
        blockedSelect.appendChild(unblockedOption);
    }
    loadUserBlockedOptions();

    document.addEventListener('click', function (event) {
        const target = event.target;

        if (target.classList.contains('item')) {
            event.preventDefault();

            const userId = target.getAttribute('data-id');

            fetch(`/api/user/${userId}`)
                .then(response => response.json())
                .then(user => {
                    document.getElementById('user-id').value = user.id;
                    document.getElementById('is-user-authorized').value = user.isAuthorized ? '0' : '1';
                    document.getElementById('is-user-blocked').value = user.isBlocked ? '0' : '1';

                    document.getElementById('users-filter').style.display = 'none';
                    document.getElementById('users-container').style.display = 'none';
                    logoutLink.style.display = 'none';

                    usersEditContainer.style.display = '';
                    backLink.style.display = '';
                });
        }
    });

    document.querySelector('#users-edit-container .user-create-form').addEventListener('submit', function (event) {
        event.preventDefault();

        const formData = new FormData(this);
        const userId = document.getElementById('user-id').value;

        fetch(`/api/user/${userId}`, {
            method: 'PUT',
            body: formData
        }).then(response => {
            if (response.ok) {
                alert('User updated successfully!');
                updateVisibility(true);
            } else {
                alert('Error updating user.');
            }
        });
    });

    // 'UserController DELETE' action logic:
    document.getElementById('delete-user').addEventListener('click', function (e) {
        e.preventDefault();
        const userId = document.getElementById('user-id').value;

        if (confirm('Are you sure you want to delete this user?')) {
            fetch(`/api/user/${userId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            }).then(response => {
                if (response.ok) {
                    alert('User deleted successfully.');
                    updateVisibility(true);
                } else {
                    alert('Error deleting user.');
                }
            });
        }
    });
});