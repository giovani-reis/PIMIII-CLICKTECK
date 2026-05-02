// Seleção de elementos
const loginBox = document.getElementById("login-box");
const registerBox = document.getElementById("register-box");
const showRegisterBtn = document.getElementById("show-register");
const showLoginBtn = document.getElementById("show-login");

// LÓGICA DE ALTERNÂNCIA (Login/Cadastro)
// Adicionado IF para evitar erro caso os botões de alternância não existam na página
if (showRegisterBtn && showLoginBtn && loginBox && registerBox) {
    showRegisterBtn.addEventListener("click", (e) => {
        e.preventDefault();
        loginBox.classList.add("hidden");
        registerBox.classList.remove("hidden");
    });

    showLoginBtn.addEventListener("click", (e) => {
        e.preventDefault();
        registerBox.classList.add("hidden");
        loginBox.classList.remove("hidden");
    });
}

// LÓGICA DE MOSTRAR SENHA
const togglePasswordBtns = document.querySelectorAll(".toggle-password");
togglePasswordBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
        const targetId = this.getAttribute("data-target");
        const input = document.getElementById(targetId);
        const icon = this.querySelector("svg");

        if (input && icon) {
            if (input.type === "password") {
                input.type = "text";
                icon.innerHTML =
                    '<path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19m-6.72-1.07a3 3 0 1 1-4.24-4.24"></path><line x1="1" y1="1" x2="23" y2="23"></line>';
            } else {
                input.type = "password";
                icon.innerHTML =
                    '<path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path><circle cx="12" cy="12" r="3"></circle>';
            }
        }
    });
});

// LÓGICA DE FILTRO (Técnicos)
const filterBtns = document.querySelectorAll(".filter-btn");
const techCards = document.querySelectorAll(".tech-card");

// Adicionado IF para rodar apenas se os filtros existirem (Página Home)
if (filterBtns.length > 0) {
    filterBtns.forEach((btn) => {
        btn.addEventListener("click", () => {
            filterBtns.forEach((b) => b.classList.remove("active"));
            btn.classList.add("active");

            const filterValue = btn.getAttribute("data-filter");

            techCards.forEach((card) => {
                const categories = card.getAttribute("data-category");
                if (filterValue === "todos" || (categories && categories.includes(filterValue))) {
                    card.style.display = "block";
                } else {
                    card.style.display = "none";
                }
            });
        });
    });
}

// LÓGICA DO MODAL (VER PERFIL)
const modal = document.getElementById("profile-modal");
const closeBtn = document.querySelector(".close-modal");
const viewBtns = document.querySelectorAll(".view-profile-btn");

if (modal && viewBtns.length > 0) {
    const modalName = document.getElementById("modal-name");
    const modalRole = document.getElementById("modal-role");
    const modalImg = document.getElementById("modal-img");

    viewBtns.forEach((btn) => {
        btn.addEventListener("click", (e) => {
            const card = e.target.closest(".tech-card");
            if (!card) return;

            const name = card.querySelector(".tech-name").innerText;
            const role = card.querySelector(".tech-role").innerText;
            const imgSrc = card.querySelector(".tech-avatar").src;

            if (modalName) modalName.innerText = name;
            if (modalRole) modalRole.innerText = role;
            if (modalImg) modalImg.src = imgSrc;

            modal.classList.add("active");
        });
    });

    if (closeBtn) {
        closeBtn.addEventListener("click", () => {
            modal.classList.remove("active");
        });
    }

    window.addEventListener("click", (e) => {
        if (e.target === modal) {
            modal.classList.remove("active");
        }
    });
}