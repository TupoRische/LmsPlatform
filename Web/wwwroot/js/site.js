document.addEventListener("DOMContentLoaded", () => {
    const loginLink = document.getElementById("login-link");
    const savedUser = localStorage.getItem("activeUser");

    if (savedUser && loginLink) {
        loginLink.textContent = `Здравей, ${savedUser}`;
    }

    const loginForm = document.getElementById("login-form");

    if (loginForm) {
        loginForm.addEventListener("submit", function (event) {
            event.preventDefault();

            const usernameInput = document.getElementById("login-username");
            const passwordInput = document.getElementById("login-password");

            const username = usernameInput.value.trim();
            const password = passwordInput.value.trim();

            if (username === "" || password === "") {
                alert("Моля, попълнете всички полета!");
                return;
            }
            localStorage.setItem("activeUser", username);

            window.location.href = "../index.html";
        });
    }
})

// търсачка
const searchBox = document.querySelector(".searchBox");
const searchToggle = document.querySelector(".search-toggle");

// Отваряне и затваряне на търсачката
searchToggle.addEventListener("click", () => {
    searchBox.classList.toggle("active");
});

// Затваряне при клик извън нея
document.addEventListener("click", (e) => {
    if (!searchBox.contains(e.target) && !searchToggle.contains(e.target)) {
        searchBox.classList.remove("active");
    }
});

const profileToggle = document.getElementById("profileToggle");
const profileMenu = document.getElementById("profileMenu");

profileToggle.addEventListener("click", () => {
    profileMenu.classList.toggle("active");
});

document.addEventListener("click", (e) => {
    if (!profileToggle.contains(e.target) &&
        !profileMenu.contains(e.target)) {
        profileMenu.classList.remove("active");
    }
});