

function changeStars(counter) {

        for (i = 0; i < 5; i++) {
            $("#star" + i).attr('class', 'fa fa-star-o fa-2x');
        }

        for (i = 0; i <= counter; i++) {
            $("#star" + i).attr('class', 'fa fa-star fa-2x');
        }
        $("#markInput").val(counter);
    }


function focusStars(counter) {
    for (i = 0; i <= counter; i++) {
        $("#star" + i).attr('class', 'fa fa-star fa-2x');
    }
}

function focusOutStars() {
    var value = $("#markInput").val();
    var counter = 0;

    if (value == 'Bad') {
        counter = 0;
    } else if (value == 'Unsatisfactory') {
        counter = 1;
    } else if (value == 'Sufficient') {
        counter = 2;
    } else if (value == 'Good') {
        counter = 3;
    } else if (value == 'VeryGood') {
        counter = 4;
    } else {
        counter = parseInt(value);
    }


    for (i = counter; i < 5; i++) {
        $("#star" + i).attr('class', 'fa fa-star-o fa-2x');
    }

    if (counter != '-1') {

        for (i = 0; i <= counter; i++) {
            $("#star" + i).attr('class', 'fa fa-star fa-2x');
        }
    }


}
