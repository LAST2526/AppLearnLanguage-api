$(document).ready(function () {
    new DataTable('#flashcardHistoryTable', {
        processing: true,
        serverSide: true,
        ordering: false,
        language: {
            url: "/lib/data-tables/en.json",
            loadingRecords: ''
        },
        paging: true,
        layout: {
            topStart: 'pageLength',
            topEnd: 'search',
            bottomStart: 'info',
            bottomEnd: 'paging'
        },
        scrollCollapse: true,
        scrollY: 200,
        deferRender: true,
        ajax: {
            url: '/FlashcardHistory/Search',
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
                data: 'courseTitle',
            },
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
            },
            {
                data: 'fileUrl',
                render: function (data, type, row) {
                    return '<p>' + row.fileName + '</p>';
                }
            }
        ],
        columnDefs: [
            { targets: "_all", className: "dt-center" }
        ],
        initComplete: function () {
            $('#dt-search-0')
                .attr('placeholder', 'Please enter a course title')
                .css({ width: '250px' });
        },
    });

    // load course list by ajax
    $.ajax({
        url: '/Courses/GetCourses',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            $('#CourseIds').append('<option value="">Please select a course</option>');
            data.forEach(function (course) {
                $('#CourseIds').append('<option value="' + course.id + '">' + course.title + '</option>');
            });
        }
    });
});

$('#flashcardHistoryCreateBtn').click(function () {
    // select first option of course list
    $('#CourseIds').val('');
    // clear file input
    $('#FileContent').val('');
    clearErrorForm();
    resetProgress();
    // show modal
    $('#flashcardHistoryModal').modal('show');
});

function clearErrorForm() {
    $('#flashcardHistoryCreateForm .is-invalid').removeClass('is-invalid');
    $('#flashcardHistoryCreateForm .invalid-feedback').text('');
    $('#flashcardHistoryCreateForm  #Error').text('');
}

$('#flashcardFormSubmitBtn').click(function () {
    clearErrorForm();
    resetProgress();
    let file = $('#FileContent')[0].files[0];
    let courseIds = $('#CourseIds').val();
    let hasError = false;
    if (courseIds === '') {
        $('#CourseIds').addClass('is-invalid');
        $('#CourseIdsInvalidFeedback').text('Please select a course');
        hasError = true;
    }
    if (file === undefined) {
        $('#FileContent').addClass('is-invalid');
        $('#FileContentInvalidFeedback').text('Please select a file');
        hasError = true;
    } else if (file.type !== 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
        $('#FileContent').addClass('is-invalid');
        $('#FileContentInvalidFeedback').text('Invalid file format, please select a .xlsx file');
        hasError = true;
    } else if (file.size > 10 * 1024 * 1024) {
        $('#FileContent').addClass('is-invalid');
        $('#FileContentInvalidFeedback').text('File size must be less than 10MB');
        hasError = true;
    }

    if (hasError) {
        return;
    }

    $('#flashcardFormSubmitBtn').prop('disabled', true);
    let formData = new FormData();
    let ids = $('#CourseIds').val();
    if (ids != null && ids != "") {
        ids.split(",").forEach((id, index) => {
            formData.append(`CourseIds[${index}]`, id);
        })
    }

    formData.append('FileContent', file);
    $.ajax({
        url: '/FlashcardHistory/Upload',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.onprogress = function (e) {
                if (e.lengthComputable) {
                    var percentComplete = Math.round(e.loaded * 100 / e.total);
                    $('#uploadFile').css('width', percentComplete + '%');
                    $('#uploadFile').text(percentComplete + '%');
                    $('#uploadFileContainer').removeClass('d-none');
                    $('#uploadFileContainer').addClass('d-flex');
                }
            };
            return xhr;
        },
        success: function (data) {
            $('#flashcardHistoryModal').modal('hide');
            $('#flashcardMessageModal').modal('show');
            $('#flashcardMessageModal .modal-body').text('Upload Complete');

            drawDataTable();
        },
        error: function (xhr, status, error) {
            let errors = xhr.responseJSON;
            for (let key in errors) {
                let error = errors[key][0];
                udpateErrorMessageToCreateForm(key, error);
            }
        },
        complete: function () {
            $('#flashcardFormSubmitBtn').prop('disabled', false);
        }
    });
});

function udpateErrorMessageToCreateForm(key, error) {
    if (key === 'Error') {
        $('#Error').text(error);
    } else {
        $('#' + key).addClass('is-invalid');
        $('#' + key + 'InvalidFeedback').text(error);
    }
}

function clearAllCreateFormErrors() {
    $('#Error').text('');
    $('#flashcardHistoryCreateForm :input').removeClass('is-invalid');
    $('#flashcardHistoryCreateForm :input').next('.invalid-feedback').text('');
}

function resetProgress() {
    $('#uploadFile').css('width', '0%');
    $('#uploadFile').text('0%');
    $('#uploadFileContainer').addClass('d-none');
    $('#uploadFileContainer').removeClass('d-flex');
}


function drawDataTable() {
    $('#flashcardHistoryTable').DataTable().ajax.reload();
}

