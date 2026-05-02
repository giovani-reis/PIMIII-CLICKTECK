document.querySelectorAll(".faq-question").forEach((button) => {
  button.addEventListener("click", () => {
    const faqItem = button.parentElement;
    faqItem.classList.toggle("active");
    const span = button.querySelector("span");
    span.innerText = faqItem.classList.contains("active") ? "−" : "+";
  });
});
