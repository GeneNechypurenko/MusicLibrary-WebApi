document.addEventListener('DOMContentLoaded', function () {
    const elementsToHide = [
        '#link-users',
        '#link-genres',
        '#link-tunes',
        '#greeting',
        '#link-logout',
        '#tunes-filter',
        '#tunes-container',
        '#users-filter',
        '#users-container'
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
                    tuneElement.innerHTML = `
                    <div class="tune-item-image">
                        <img src="${tune.posterUrl}" alt="${tune.title}" />
                    </div>
                    <div class="tune-box-content">
                        <div class="tune-item-description">
                            <a class="link_type_d" href="/create">"${tune.performer} - ${tune.title}"</a>
                        </div>
                        <div class="tune-item-audio">
                            <audio controls>
                                <source src="${tune.fileUrl}" type="audio/mpeg">
                            </audio>
                        </div>
                    </div>
                `;
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
});
