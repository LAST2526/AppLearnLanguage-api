import { Spinner } from '/lib/spin/spin.js';

let opts = {
    lines: 12, // The number of lines to draw
    length: 21, // The length of each line
    width: 6, // The line thickness
    radius: 7, // The radius of the inner circle
    scale: 1.15, // Scales overall size of the spinner
    corners: 1, // Corner roundness (0..1)
    speed: 1, // Rounds per second
    rotate: 0, // The rotation offset
    animation: 'spinner-line-shrink', // The CSS animation name for the lines
    direction: 1, // 1: clockwise, -1: counterclockwise
    color: '#4a4a4a', // CSS color or array of colors
    fadeColor: 'transparent', // CSS color or array of colors
    top: '50%', // Top position relative to parent
    left: '50%', // Left position relative to parent
    shadow: '0 0 1px transparent', // Box-shadow for the lines
    zIndex: 2000000000, // The z-index (defaults to 2e9)
    className: 'spinner', // The CSS class to assign to the spinner
    position: 'absolute', // Element positioning
};

let spinner = new Spinner(opts);

$(function () {
    new DataTable('#coursesTable', {
        processing: true,
        serverSide: true,
        scrollY: 200,
        deferRender: true,
        ordering: false,
        language: {
            url: "/lib/data-tables/en.json",
            loadingRecords: ''
        },
        paging: false,
        info: false,    
        lengthChange: false,  
        layout: {
            topEnd: 'search',
        },
        scrollCollapse: true,
        ajax: {
            url: '/Courses/Search',
            type: 'GET',
            dataType: "json",
            dataSrc: {
                data: 'data',
                draw: 'request',
                recordsTotal: 'totalRecords',
                recordsFiltered: 'recordsFiltered'
            },
            data: function (d) {
                return {
                    keyword: d.search.value,
                    page: (d.start / d.length) + 1,
                    size: d.length
                };
            }
        },
        columns: [
            {
                data: null,
                orderable: false,
                className: 'select-checkbox',
                defaultContent: '',
                render: function (data, type, row) {
                    return '<label class="select-checkbox-label" for="select-checkbox-' + row.id + '"><input type="checkbox" class="course-checkbox" value="' + row.id + '" id="select-checkbox-' + row.id + '"></label>';
                }
            },
            { data: 'title' },
            {
                data: 'createdDate',
                render: function (data, type, row) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleDateString('en-CA', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit'
                    });
                }
            }
        ],
        columnDefs: [
            { targets: "_all", className: "dt-center" }
        ],
        initComplete: function () {
            $('#dt-search-0')
                .attr('placeholder', 'Please enter title')
                .css({ width: '250px' });
        },
        // event after draw
        drawCallback: function () {
            $('.course-checkbox').on('change', function () {
                let checkedCount = $('#coursesTable tbody').find('.course-checkbox:checked').length;
                let totalCount = $('#coursesTable tbody').find('.course-checkbox').length;
                if (checkedCount === totalCount) {
                    $('#selectAllCoursesCheckbox').prop('checked', true);
                } else {
                    $('#selectAllCoursesCheckbox').prop('checked', false);
                }

                if (checkedCount > 0) {
                    $('#deleteCourseBtn').removeClass('d-none');
                } else {
                    $('#deleteCourseBtn').addClass('d-none');
                }

                if (checkedCount == 1) {
                    $('#editCourseBtn').removeClass('d-none');
                } else {
                    $('#editCourseBtn').addClass('d-none');
                }
            });

            $('#coursesTable tbody tr td').not('.select-checkbox').on('click', function (e) {
                $('.course-checkbox').prop('checked', false);
                const checkbox = $(this).parent().find('.course-checkbox');
                checkbox.prop('checked', !checkbox.prop('checked')).trigger('change');
            });

            $('#editCourseBtn').addClass('d-none');
            $('#deleteCourseBtn').addClass('d-none');
            $('#selectAllCoursesCheckbox').prop('checked', false);
        }
    });

    $('#courseCreateForm :input').attr('autocomplete', 'off');

});

$('#createCourseBtn').on('click', function () {
    switchForm('create', 'form');
    localStorage.setItem('courseState', 'create');
    clearAllContentOfCreateForm();
    clearAllCreateFormErrors();
    enableAllCreateFormElements();

    $('#courseCreateModal').modal('show');
    localStorage.setItem('courseState', 'create');
});

$('#courseCreateBtn').on('click', function () {
    let target = document.getElementById('courseCreateModal');
    spinner.spin(target);

    var formData = $('#courseCreateForm').serialize();
    disableAllCreateFormElements();
    clearAllCreateFormErrors();
    $.ajax({
        url: '/Courses/ValidateCreate',
        type: 'POST',
        data: formData,
        contentType: 'application/x-www-form-urlencoded',
        success: function (response) {
            spinner.stop();
            disableAllCreateFormInputs();
            enableAllCreateFormButtons();
            switchForm(localStorage.getItem('courseState'), 'confirm');
        },
        error: function (xhr, status, error) {
            spinner.stop();
            enableAllCreateFormElements();
            let responseJson = xhr.responseJSON;
            let errors = responseJson.errors;
            for (let key in errors) {
                let error = errors[key][0];
                udpateErrorMessageToCreateForm(key, error);
            }
        }
    });
});

function enableAllCreateFormElements() {
    enableAllCreateFormInputs();
    enableAllCreateFormButtons();
}

function clearAllContentOfCreateForm() {
    $('#courseCreateForm input').val('');
    $('#courseCreateForm input[type="checkbox"]').prop('checked', false);
}

function disableAllCreateFormElements() {
    disableAllCreateFormInputs();
    disableAllCreateFormButtons();
}

function disableAllCreateFormInputs() {
    $('#courseCreateModal input').prop('disabled', true);
    $('#courseCreateModal select').prop('disabled', true);
    $('#courseCreateModal textarea').prop('disabled', true);
}

function disableAllCreateFormButtons() {
    $('#courseCreateBtn').prop('disabled', true);
    $('#courseConfirmCreateBtn').prop('disabled', true);
}

function clearAllCreateFormErrors() {
    $('#courseCreateModal input').removeClass('is-invalid');
    $('#courseCreateModal textarea').removeClass('is-invalid');
    $('#courseCreateModal select').removeClass('is-invalid');
    $('#courseCreateModal .invalid-feedback').text('');
}

function enableAllCreateFormInputs() {
    $('#courseCreateModal input').prop('disabled', false);
    $('#courseCreateModal button').prop('disabled', false);
    $('#courseCreateModal select').prop('disabled', false);
    $('#courseCreateModal textarea').prop('disabled', false);
}

function enableAllCreateFormButtons() {
    $('#courseCreateBtn').prop('disabled', false);
    $('#courseConfirmCreateBtn').prop('disabled', false);
}

$('#courseBackToCreateBtn').on('click', function () {
    let mode = localStorage.getItem('courseState');
    switchForm(mode, 'form');
});

function switchForm(mode, step) {
    setModalText(mode, step);
    if (step === 'form') {
        $('#courseCreateBtn').removeClass('d-none');
        $('#courseConfirmCreateBtn').addClass('d-none');
        $('#courseBackToCreateBtn').addClass('d-none');
        enableAllCreateFormInputs();
        enableAllCreateFormButtons();
    } else if (step === 'confirm') {
        $('#courseCreateBtn').addClass('d-none');
        $('#courseConfirmCreateBtn').removeClass('d-none');
        $('#courseBackToCreateBtn').removeClass('d-none');
        disableAllCreateFormInputs();
    }
}

function setModalText(mode, step) {
    if (mode === 'create') {
        if (step === 'form') {
            $('#courseCreateModal .modal-title').text('Create Course');
            $('#courseCreateBtn').text('Create');
        } else if (step === 'confirm') {
            $('#courseConfirmCreateBtn').text('Confirm');
        }
    } else if (mode === 'edit') {
        if (step === 'form') {
            $('#courseCreateModal .modal-title').text('Edit Course');
            $('#courseCreateBtn').text('Update');
        } else if (step === 'confirm') {
            $('#courseConfirmCreateBtn').text('Confirm');
        }
    }
}

function checkCourseCreateFormChanged() {
    let courseState = localStorage.getItem('courseState');
    let isChanged = false;

    if (courseState == 'create') {
        $('#courseCreateForm :input').not('input[type="checkbox"]').each(function () {
            if ($(this).val() !== '' && $(this).val() !== null) {
                isChanged = true;
            }
        });
        if (isChanged) {
            return true;
        }
        if ($('#courseCreateForm input[type="checkbox"]').is(':checked')) {
            return true;
        }
    } else if (courseState == 'edit') {
        let titleInput = $('#courseCreateForm input[name="Title"]');
        let languageSelect = [];
        if ($('#courseCreateForm #vietnameseLanguage').is(':checked')) {
            languageSelect.push('Vietnamese');
        }
        if ($('#courseCreateForm #englishLanguage').is(':checked')) {
            languageSelect.push('English');
        }
        let courseEditData = JSON.parse(localStorage.getItem('courseEditData'));
    }

    return false;
}

$('#courseCreateModal .btn-close, #courseCreateModal #courseCloseBtn').on('click', function (e) {
    if (checkCourseCreateFormChanged()) {
        e.preventDefault();
        if (confirm('Your changes have not been saved.\nAre you sure you want to close?')) {
            $('#courseCreateModal').modal('hide');
        }
    } else {
        $('#courseCreateModal').modal('hide');
    }
});

// event after close modal
$('#courseCreateModal').on('hidden.bs.modal', function () {
    localStorage.removeItem('courseState');
});

function compare(v1, v2) {
    let v1_cp = null;
    let v2_cp = null;
    if (v1 != null) {
        v1_cp = String(v1);
    } else {
        v1_cp = "";
    }
    if (v2 != null) {
        v2_cp = String(v2);
    } else {
        v2_cp = "";
    }

    return v1_cp == v2_cp;
}

// Handle page navigation
$(window).on('beforeunload', function (e) {
    const isModalOpen = $('#courseCreateModal').is(':visible');

    if (isModalOpen && checkCourseCreateFormChanged()) {
        e.preventDefault();
        return 'Your changes have not been saved.\nAre you sure you want to leave the page?';
    }
});

$('#courseConfirmCreateBtn').on('click', function () {
    const formData = {};
    $('#courseCreateForm :input').each(function () {
        const name = $(this).attr('name');
        if (name) {
            if (this.tagName === 'INPUT' && $(this).attr('type') === 'checkbox') {
                formData[name] = $(this).is(':checked');
            } else {
                formData[name] = $(this).val();
            }
        }
    });

    let courseState = localStorage.getItem('courseState');
    $.ajax({
        url: courseState == 'create' ? '/Courses/Create' : '/Courses/Update',
        type: 'POST',
        data: formData,
        contentType: 'application/x-www-form-urlencoded',
        success: function (response) {
            // close modal
            $('#courseCreateModal').modal('hide');
            // show modal success
            $('#courseCreateResultModal').modal('show');
            $('#courseCreateResultTitle').text('Success');
            // show success message
            $('#courseCreateResultMessage').text(courseState == 'create' ? 'The course has been created.' : 'The course has been updated.');
            // reload datatable
            $('#coursesTable').DataTable().ajax.reload();
            $('#coursesTable').DataTable().draw();

        },
        error: function (xhr, status, error) {
            // close modal
            $('#courseCreateModal').modal('hide');
            // show modal error
            $('#courseCreateResultModal').modal('show');
            $('#courseCreateResultTitle').text('Error');
            // show error message
            $('#courseCreateResultMessage').text(courseState == 'create' ? 'Course creation failed.' : 'Course update failed.');
        }
    });
});

$('#selectAllCoursesCheckbox').on('change', function () {
    var isChecked = $(this).prop('checked');
    $('.course-checkbox').prop('checked', isChecked);
    if (isChecked) {
        $('#deleteCourseBtn').removeClass('d-none');
        $('#editCourseBtn').addClass('d-none');
    } else {
        $('#deleteCourseBtn').addClass('d-none');
    }
});

$('#deleteCourseBtn').on('click', function () {
    $('#courseDeleteConfirmModal').modal('show');
});

$('#courseDeleteConfirmBtn').on('click', function () {
    $('#courseDeleteConfirmModal').modal('hide');

    let selectedCourseIds = [];
    $('.course-checkbox:checked').each(function () {
        let ids = $(this).val().split(',');
        selectedCourseIds = selectedCourseIds.concat(ids);
    });

    $.ajax({
        url: '/Courses/Delete',
        type: 'POST',
        data: { ids: selectedCourseIds },
        success: function (response) {
            $('#courseCreateResultModal').modal('show');
            $('#courseCreateResultTitle').text('Success');
            // show success message
            $('#courseCreateResultMessage').text('The course has been deleted.');
            $('#coursesTable').DataTable().ajax.reload();
            $('#coursesTable').DataTable().draw();
        },
        error: function (xhr, status, error) {
            $('#courseCreateResultModal').modal('show');
            $('#courseCreateResultTitle').text('Error');
            // show error message
            $('#courseCreateResultMessage').text('Course deletion failed.');
        }
    });
});

$('#editCourseBtn').on('click', function () {
    switchForm('edit', 'form');
    localStorage.setItem('courseState', 'edit');
    // get selected course id
    let selectedCourseId = $('.course-checkbox:checked').val();
    if (selectedCourseId) {
        $('#courseCreateModal').modal('show');
        clearAllContentOfCreateForm();
        clearAllCreateFormErrors();
        enableAllCreateFormElements();
        // show spinner
        let target = document.getElementById('courseCreateModal');
        spinner.spin(target);
        // get course data
        $.ajax({
            url: '/Courses/GetByIds',
            type: 'GET',
            data: { ids: selectedCourseId },
            success: function (response) {
                // stop spinner
                spinner.stop();
                // set course data
                $('#courseCreateForm input[name="Title"]').val(response.title);
                $('#courseCreateForm input[name="Id"]').val(response.id);

                localStorage.setItem('courseEditData', JSON.stringify(response));
            },
            error: function (xhr, status, error) {
                // stop spinner
                spinner.stop();
                // show error message
                $('#courseCreateResultModal').modal('show');
                $('#courseCreateResultTitle').text('error');
                $('#courseCreateResultMessage').text('Response course failed');
            }
        });
    }
});
