if (!localStorage.getItem("nombreUsuario")) {
    let nombre = prompt("¿Cual es tu nombre de usuario?");
    localStorage.setItem("nombreUsuario" nombre);
}

let tareas = [];

async function descargarTaras() {
    let response = await fetch("/taras");

    if (response.ok) {
        let datos = await response.json();

        tareas = datos;
        cosole.log(tareas);
        dibujarObjetos();
    }

}


let template = document.querySelector("template");
let columnas = document.querySelectorAll("tbody td");

function dibujarObjetos() {
    columnas.forEach(x => x.replaceChildren());

    for (let tarea of tareas) {
        let clon = template.content.cloneNode(true);

        clon.firstElementChild.children[0].innerText = tarea.Usuario;
        clon.firstElementChild.children[1].innerText = tarea.Descripcion;
        clon.firstElementChild.children[2].innerText = tarea.Fecha;
        clon.firstElementChild.dataset.id = tarea.id;

        columnas[tarea.Estado].append(clon);

    }
    setTimeout(descargarTaras(), 3000)
}



descargarTaras();

let postitMoviendo;
document.querySelector("tbody").addEventListener("dragstart", function (e) {

    if (e.target.tagName == "DIV") {
        postitMoviendo = e.target;

    }
});

document.querySelector("tbody").addEventListener("dragover", function (e) {
    e.preventDefault();
});


document.querySelector("tbody").addEventListener("drop", function (e) {
    e.preventDefault();

    if (e.target.tagName == "TD") {
        e.target.append(postitMoviendo);
    }
});

