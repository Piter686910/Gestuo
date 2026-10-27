// Questo codice parte appena la pagina è caricata
document.addEventListener("DOMContentLoaded", function () {

    // Cerca tutti gli elementi con la classe 'data-auto' e aggiunge l'evento
    var inputs = document.querySelectorAll('.data-auto');

    inputs.forEach(function (input) {
        input.addEventListener('keyup', function () {
            formattaData(this);
        });
    });
});
document.addEventListener("DOMContentLoaded", function () {

    // Cerca tutti gli elementi con la classe 'data-auto-trattino' e aggiunge l'evento
    var inputs = document.querySelectorAll('.data-auto-trattino');

    inputs.forEach(function (input) {
        input.addEventListener('keyup', function () {
            formattaDataTrattini(this);
        });
    });
});

// La funzione aggiunge 30 giorni alla data inserita nella TextBox 
function aggiungi30Giorni(source, targetId) {
    var dataString = source.value;

    if (dataString.length === 10) {
        // Parsing della data
        var parts = dataString.split('/');
        var giorno = parseInt(parts[0], 10);
        var mese = parseInt(parts[1], 10) - 1; // Gennaio è 0
        var anno = parseInt(parts[2], 10);

        var dataObj = new Date(anno, mese, giorno);

        // 1. Aggiungi 30 giorni
        dataObj.setDate(dataObj.getDate() + 30);

        // 2. CICLO DI CONTROLLO FESTIVI
        // Continua ad aggiungere 1 giorno finché la data è un festivo o domenica
        while (isFestivo(dataObj)) {
            dataObj.setDate(dataObj.getDate() + 1);
        }

        // Formattazione per output
        var newGiorno = dataObj.getDate();
        var newMese = dataObj.getMonth() + 1;
        var newAnno = dataObj.getFullYear();

        if (newGiorno < 10) newGiorno = '0' + newGiorno;
        if (newMese < 10) newMese = '0' + newMese;

        var dataFinale = newGiorno + '/' + newMese + '/' + newAnno;

        // Scrittura nella textbox target (assumendo che hai usato ClientIDMode="Static")
        var targetBox = document.getElementById(targetId);
        if (targetBox) {
            targetBox.value = dataFinale;
        } else {
            console.error("TextBox destinazione non trovata: " + targetId);
        }
    }
}

// Funzione ausiliaria che dice se è festa (True) o lavorativo (False)
function isFestivo(data) {
    var d = data.getDate();
    var m = data.getMonth() + 1;
    var dayOfWeek = data.getDay(); // 0 = Domenica, 1 = Lunedì...

    // A. Controllo se è DOMENICA (se vuoi escludere anche Sabato metti || dayOfWeek === 6)
    if (dayOfWeek === 0) return true;

    // Formatto gg/mm per cercare nell'elenco
    var giornoMese = (d < 10 ? '0' + d : d) + '/' + (m < 10 ? '0' + m : m);

    // B. Elenco Feste Fisse Italiane
    var festeFisse = [
        "01/01", // Capodanno
        "06/01", // Epifania
        "25/04", // Liberazione
        "01/05", // Festa del Lavoro
        "02/06", // Repubblica
        "15/08", // Ferragosto
        "19/09", // San Gennaro (Patrono Napoli) - Rimuovi se non serve
        "01/11", // Ognissanti
        "08/12", // Immacolata
        "25/12", // Natale
        "26/12"  // Santo Stefano
    ];

    if (festeFisse.indexOf(giornoMese) > -1) {
        return true;
    }

    // Nota: Il calcolo di Pasqua e Pasquetta in JS è complesso.
    // Solitamente per scadenze amministrative brevi si accetta il rischio 
    // oppure si controlla manualmente se cade in Marzo/Aprile.

    return false;
}

function formattaDataTrattini(event, input) {
    // Permetti sempre tasti di controllo come Backspace, Canc, Frecce
    if (event.key === 'Backspace' || event.key === 'Delete' || event.key === 'ArrowLeft' || event.key === 'ArrowRight') {
        return;
    }

    // Blocca l'inserimento se non è un numero
    if (/\D/.test(event.key)) {
        event.preventDefault();
        return;
    }

    var valore = input.value;

    // Se l'utente ha scritto 2 cifre (gg), aggiunge il trattino prima del terzo carattere
    if (valore.length === 2 || valore.length === 5) {
        input.value = valore + '-';
    }
}
// La funzione inserisce / automatici nella data
function formattaData(input) {
    var numeri = input.value.replace(/\D/g, '');

    if (numeri.length > 8) {
        numeri = numeri.substr(0, 8);
    }

    if (numeri.length >= 5) {
        input.value = numeri.substr(0, 2) + '/' + numeri.substr(2, 2) + '/' + numeri.substr(4);
    }
    else if (numeri.length >= 3) {
        input.value = numeri.substr(0, 2) + '/' + numeri.substr(2);
    }
    else {
        input.value = numeri;
    }
}
function PulisciSeSbagliato(txt) {
    if (/\D/.test(txt.value)) {
        txt.value = "";
        // alert("Inserire solo numeri.");
    }
}
