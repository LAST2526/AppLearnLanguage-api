$(document).ready(function () {
    new DataTable('#audioGenHistoryTable', {
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
            url: '/AudioGenHistory/Search',
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
                data: 'courseTitle'
            },
            {
                data: 'fileUrl',
                render: function (data, type, row) {
                    return '<p>' + row.fileName + '</p>';
                }
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
        ],
        initComplete: function () {
            $('#dt-search-0')
                .attr('placeholder', 'Please enter a file name')
                .css({ width: '250px' });
        },
    });
});

$('#audioGenHistoryCreateBtn').click(function () {
    $('#FileContent').val('');
    clearErrorForm();
    resetProgress();
    $('#audioGenHistoryModal').modal('show');
});

function clearErrorForm() {
    $('#audioGenHistoryCreateForm .is-invalid').removeClass('is-invalid');
    $('#audioGenHistoryCreateForm .invalid-feedback').text('');
    $('#audioGenHistoryCreateForm #Error').text('');
}

$('#audioGenQRCodeFormSubmitBtn').click(function () {
    clearErrorForm();
    resetProgress();
    let hasError = false;
    if ($('#CourseIds').val() === '') {
        $('#CourseIds').addClass('is-invalid');
        $('#CourseIdsInvalidFeedback').text('Please select a course');
        hasError = true;
    }
    let file = $('#FileContent')[0].files[0];
    if (file === undefined) {
        $('#FileContent').addClass('is-invalid');
        $('#FileContentInvalidFeedback').text('Please select a file');
        hasError = true;
    } else if (file.type !== 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
        $('#FileContent').addClass('is-invalid');
        $('#FileContentInvalidFeedback').text('The file format is invalid. Please select an .xlsx file');
        hasError = true;
    } else if (file.size > 10 * 1024 * 1024) {
        $('#FileContent').addClass('is-invalid');
        $('#FileContentInvalidFeedback').text('The file size must be 10MB or less');
        hasError = true;
    }

    if (hasError) {
        return;
    }

    $('#audioGenQRCodeFormSubmitBtn').prop('disabled', true);
    let formData = new FormData();
    let ids = $('#CourseIds').val();
    if (ids != null && ids != "") {
        ids.split(",").forEach((id, index) => {
            formData.append(`CourseIds[${index}]`, id);
        })
    }
    formData.append('FileContent', file);
    $.ajax({
        url: '/audioGenHistory/Upload',
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
            $('#audioGenHistoryModal').modal('hide');
            $('#audioGenQRCodeMessageModal').modal('show');
            $('#audioGenQRCodeMessageModal .modal-body').text('Upload completed');
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
            $('#audioGenQRCodeFormSubmitBtn').prop('disabled', false);
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

function resetProgress() {
    $('#uploadFile').css('width', '0%');
    $('#uploadFile').text('0%');
    $('#uploadFileContainer').addClass('d-none');
    $('#uploadFileContainer').removeClass('d-flex');
}

function drawDataTable() {
    $('#audioGenHistoryTable').DataTable().ajax.reload();
}

