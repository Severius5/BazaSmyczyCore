﻿$(function () {
    $('.modal').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });
});