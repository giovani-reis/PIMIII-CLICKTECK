const mobileMenu = document.getElementById("mobile-menu");
const navLinks = document.getElementById("nav-links");

mobileMenu.addEventListener("click", () => {
  navLinks.classList.toggle("active");
});

document.querySelectorAll(".nav-links a").forEach((link) => {
  link.addEventListener("click", () => {
    navLinks.classList.remove("active");
  });
});
