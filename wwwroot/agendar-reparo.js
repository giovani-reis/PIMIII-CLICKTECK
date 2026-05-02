// Inicialização dos dados
window.addEventListener("DOMContentLoaded", () => {
  const techName = localStorage.getItem("techName");
  const techImg = localStorage.getItem("techImg");

  document.getElementById("chat-tech-name").innerText = techName
    ? techName
    : "Técnico Não Identificado";
  if (techImg) {
    const imgElement = document.getElementById("chat-tech-avatar");
    imgElement.src = techImg;
    imgElement.style.display = "block";
  } else {
    document.getElementById("chat-tech-avatar").style.display = "block";
    // Mostra placeholder
  }
});

const chatInput = document.getElementById("chat-input");
const btnSend = document.getElementById("btn-send");
const chatMessages = document.getElementById("chat-messages");
const btnAttach = document.getElementById("btn-attach");
const fileInput = document.getElementById("file-input");

// Função universal para injetar mensagens
function adicionarMensagem(conteudo, tipo = "user", isImage = false) {
  if (conteudo.trim() !== "") {
    const data = new Date();
    const hora =
      data.getHours().toString().padStart(2, "0") +
      ":" +
      data.getMinutes().toString().padStart(2, "0");

    const novaMensagem = document.createElement("div");
    novaMensagem.classList.add("message", `message-${tipo}`);

    // Se for só imagem, adiciona a classe especial para tirar o padding e o "rabinho"
    const bubbleClass = isImage
      ? "message-bubble bubble-image-only"
      : "message-bubble";

    novaMensagem.innerHTML = `
            <div class="${bubbleClass}">${conteudo}</div>
            <span class="message-time">${hora}</span>
        `;
    chatMessages.appendChild(novaMensagem);
    chatMessages.scrollTop = chatMessages.scrollHeight;
  }
}

// Lógica de Envio de Texto
function enviarTexto() {
  if (chatInput.value.trim() !== "") {
    adicionarMensagem(chatInput.value, "user", false);
    chatInput.value = "";
  }
}

btnSend.addEventListener("click", enviarTexto);

chatInput.addEventListener("keypress", (e) => {
  if (e.key === "Enter") enviarTexto();
});

// Lógica de Upload de Imagem
btnAttach.addEventListener("click", () => fileInput.click());

fileInput.addEventListener("change", function () {
  if (this.files && this.files.length > 0) {
    for (let i = 0; i < this.files.length; i++) {
      const file = this.files[i];

      if (file.type.startsWith("image/")) {
        const reader = new FileReader();

        reader.onload = function (evento) {
          const imgHTML = `<img src="${evento.target.result}" class="message-image" alt="Foto do aparelho">`;
          // Passa 'true' no terceiro parâmetro para aplicar o estilo sem bordas
          adicionarMensagem(imgHTML, "user", true);
        };

        reader.readAsDataURL(file);
      }
    }
    this.value = "";
  }
});
