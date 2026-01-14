import { Spinner } from '/lib/spin/spin.js';

const csrfToken = document.querySelector('meta[name="csrf-token"]').getAttribute('content');

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

$(document).ready(function () {
    new DataTable('#notificationsTable', {
        processing: true,
        serverSide: true,
        scrollY: 200,
        deferRender: true,
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
        ajax: {
            url: '/Notification/Search',
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
                    return '<label class="select-checkbox-label" for="select-checkbox-' + row.id + '"><input type="checkbox" class="notification-checkbox" value="' + row.id + '" id="select-checkbox-' + row.id + '"></label>';
                }
            },
            { data: 'title' },
            {
                data: 'body',
                render: function (data, type, row) {
                    if (type === 'display') {
                        const div = document.createElement('div');
                        div.innerHTML = data || '';
                        const textContent = div.textContent || div.innerText || '';

                        const maxLength = 100;
                        const shortText = textContent.length > maxLength
                            ? textContent.substring(0, maxLength) + '...'
                            : textContent;

                        return `<span title="${textContent.replace(/"/g, '&quot;')}">${shortText}</span>`;
                    }

                    return data;
                }
            },
            {
                data: 'scheduledTime',
                render: function (data, type, row) {
                    if (!data) return '';
                    const date = new Date(data);
                    return date.toLocaleDateString('en-CA', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        hour: '2-digit',
                        minute: '2-digit',
                        hour12: false
                    });
                }
            },
            {
                data: 'isSpecial',
                render: function (data, type, row) {
                    return data ? '〇' : '－';
                },
                className: 'text-center'
            },
            { data: 'courseTitle' },
            {
                data: 'isSent',
                render: function (data, type, row) {
                    return data ? '〇' : '－';
                },
                className: 'text-center'
            },
        ],
        columnDefs: [
            {
                targets: '_all',
                className: 'notification-td-max-width text-wrap'
            }
        ],
        initComplete: function () {
            $('#dt-search-0')
                .attr('placeholder', 'Please enter a title')
                .css({ width: '250px' });
        },
        // event after draw
        drawCallback: function () {
            $('.notification-checkbox').on('change', function () {
                let checkedCount = $('#notificationsTable tbody').find('.notification-checkbox:checked').length;
                let totalCount = $('#notificationsTable tbody').find('.notification-checkbox').length;
                if (checkedCount === totalCount) {
                    $('#selectAllNotificationsCheckbox').prop('checked', true);
                } else {
                    $('#selectAllNotificationsCheckbox').prop('checked', false);
                }

                if (checkedCount > 0) {
                    $('#deleteNotificationBtn').removeClass('d-none');
                } else {
                    $('#deleteNotificationBtn').addClass('d-none');
                }

                if (checkedCount == 1) {
                    $('#editNotificationBtn').removeClass('d-none');
                } else {
                    $('#editNotificationBtn').addClass('d-none');
                }
            });

            $('#notificationsTable tbody tr td').not('.select-checkbox').on('click', function (e) {
                $('.notification-checkbox').prop('checked', false);
                const checkbox = $(this).parent().find('.notification-checkbox');
                checkbox.prop('checked', !checkbox.prop('checked')).trigger('change');
            });

            $('#editNotificationBtn').addClass('d-none');
            $('#deleteNotificationBtn').addClass('d-none');
            $('#selectAllNotificationsCheckbox').prop('checked', false);
        }
    });

    $('#notificationCreateForm :input').attr('autocomplete', 'off');

    $('#notificationCreateModal').modal({
        focus: false
    });

    $('#notificationCreateForm #IsSpecial').on('change', function () {
        if ($(this).is(':checked')) {
            $('#courseTitleWrapper').removeClass('d-none');
        } else {
            $('#courseTitleWrapper').addClass('d-none');
        }
    });
});

$('#createNotificationBtn').click(function () {
    switchForm('create', 'form');
    localStorage.setItem('notificationState', 'create');
    clearAllContentOfCreateForm();
    clearAllCreateFormErrors();
    enableAllCreateFormElements();
    $('#notificationCreateModal').modal('show');
    localStorage.setItem('notificationState', 'create');
});

$('#notificationCreateBtn').click(function () {
    const bodyEditorData = notificationBodyEditor.getData();
    $('#notificationCreateForm textarea[name="Body"]').val(bodyEditorData);

    let target = document.getElementById('notificationCreateModal');
    spinner.spin(target);

    convertLocalDateTimeToOffset('#ScheduledTimeInput', '#ScheduledTimeOffset');

    var formData = $('#notificationCreateForm').serialize();
    disableAllCreateFormElements();
    clearAllCreateFormErrors();
    $.ajax({
        url: '/Notification/ValidateCreate',
        type: 'POST',
        data: formData,
        contentType: 'application/x-www-form-urlencoded',
        success: function (response) {
            spinner.stop();
            disableAllCreateFormInputs();
            enableAllCreateFormButtons();
            switchForm(localStorage.getItem('notificationState'), 'confirm');
        },
        error: function (xhr, status, error) {
            spinner.stop();
            enableAllCreateFormElements();
            let responseJson = xhr.responseJSON;
            let errors = responseJson.errors;
            for (let key in errors) {
                let error = errors[key][0];
                udpateErrorMessageToCreateForm(key, error);
                if (key == "ScheduledTime") {
                    udpateErrorMessageToCreateForm("ScheduledTimeInput", error);
                }
            }
        }
    });
});

function convertLocalDateTimeToOffset(sourceSelector, hiddenSelector) {
    const val = $(sourceSelector).val(); // "2025-06-24T08:15"
    if (!val) return;

    const d = new Date(val);
    const offset = -d.getTimezoneOffset();
    const sign = offset >= 0 ? '+' : '-';
    const h = String(Math.floor(Math.abs(offset) / 60)).padStart(2, '0');
    const m = String(Math.abs(offset) % 60).padStart(2, '0');

    const offsetStr = `${sign}${h}:${m}`;
    const result = `${val}:00${offsetStr}`;

    $(hiddenSelector).val(result);
}

function enableAllCreateFormElements() {
    enableAllCreateFormInputs();
    enableAllCreateFormButtons();
}

function clearAllContentOfCreateForm() {
    // Clear all text inputs (excluding hidden)
    $('#notificationCreateForm input[type="text"]').val('');

    // Clear datetime-local input
    $('#ScheduledTimeInput').val('');

    // Clear hidden inputs
    $('#notificationCreateForm input[type="hidden"]').val('');

    // Uncheck all checkboxes
    $('#notificationCreateForm input[type="checkbox"]').prop('checked', false);

    // Clear all textareas
    $('#notificationCreateForm textarea').val('');

    // Clear validation feedback
    $('#notificationCreateForm .is-invalid').removeClass('is-invalid');
    $('#notificationCreateForm .invalid-feedback').text('');

    $('#notificationCreateForm #courseTitleWrapper').addClass('d-none');

    // Clear general error message
    $('#Error').text('');
    if (typeof notificationBodyEditor !== 'undefined' && notificationBodyEditor) {
        notificationBodyEditor.setData('');
    }
}

function disableAllCreateFormElements() {
    disableAllCreateFormInputs();
    disableAllCreateFormButtons();
}

function disableAllCreateFormInputs() {
    $('#notificationCreateModal input').prop('disabled', true);
    $('#notificationCreateModal select').prop('disabled', true);
    $('#notificationCreateModal textarea').prop('disabled', true);
    notificationBodyEditor.enableReadOnlyMode('form-disable');
}

function disableAllCreateFormButtons() {
    $('#notificationCreateBtn').prop('disabled', true);
    $('#notificationConfirmCreateBtn').prop('disabled', true);
}

function udpateErrorMessageToCreateForm(key, error) {
    $('#' + key).addClass('is-invalid');
    let input = $('#' + key.charAt(0).toLowerCase() + key.slice(1) + 'InvalidFeedback');
    input.text(error);
}

function clearAllCreateFormErrors() {
    $('#notificationCreateModal input').removeClass('is-invalid');
    $('#notificationCreateModal textarea').removeClass('is-invalid');
    $('#notificationCreateModal select').removeClass('is-invalid');
    $('#notificationCreateModal .invalid-feedback').text('');
}

function enableAllCreateFormInputs() {
    $('#notificationCreateModal input').prop('disabled', false);
    $('#notificationCreateModal button').prop('disabled', false);
    $('#notificationCreateModal select').prop('disabled', false);
    $('#notificationCreateModal textarea').prop('disabled', false);
    notificationBodyEditor.disableReadOnlyMode('form-disable');
}

function enableAllCreateFormButtons() {
    $('#notificationCreateBtn').prop('disabled', false);
    $('#notificationConfirmCreateBtn').prop('disabled', false);
}

$('#notificationBackToCreateBtn').click(function () {
    let mode = localStorage.getItem('notificationState');
    switchForm(mode, 'form');
});

function switchForm(mode, step) {
    setModalText(mode, step);
    if (step === 'form') {
        $('#notificationCreateBtn').removeClass('d-none');
        $('#notificationConfirmCreateBtn').addClass('d-none');
        $('#notificationBackToCreateBtn').addClass('d-none');
        enableAllCreateFormInputs();
        enableAllCreateFormButtons();
    } else if (step === 'confirm') {
        $('#notificationCreateBtn').addClass('d-none');
        $('#notificationConfirmCreateBtn').removeClass('d-none');
        $('#notificationBackToCreateBtn').removeClass('d-none');
        disableAllCreateFormInputs();
    }
}

function setModalText(mode, step) {
    if (mode === 'create') {
        if (step === 'form') {
            $('#notificationCreateModal .modal-title').text('新しい通知を作成する');
            $('#notificationCreateBtn').text('作成');
        } else if (step === 'confirm') {
            $('#notificationConfirmCreateBtn').text('作成を確認');
        }
    } else if (mode === 'edit') {
        if (step === 'form') {
            $('#notificationCreateModal .modal-title').text('通知情報を編集する');
            $('#notificationCreateBtn').text('更新');
        } else if (step === 'confirm') {
            $('#notificationConfirmCreateBtn').text('更新を確認');
        }
    }
}

function checkNotificationCreateFormChanged() {
    let notificationState = localStorage.getItem('notificationState');
    let isChanged = false;

    if (notificationState == 'create') {
        $('#notificationCreateForm :input').not('input[type="checkbox"]').each(function () {
            if ($(this).val() !== '' && $(this).val() !== null) {
                isChanged = true;
            }
        });
        if (isChanged) {
            return true;
        }
        if ($('#notificationCreateForm input[type="checkbox"]').is(':checked')) {
            return true;
        }
    } else if (notificationState == 'edit') {
        let titleInput = $('#notificationCreateForm input[name="Title"]');
        let bodyInput = $('#notificationCreateForm textarea[name="Body"]');

        let notificationEditData = JSON.parse(localStorage.getItem('notificationEditData'));

        if (!compare(titleInput.val(), notificationEditData.title) ||
            !compare(bodyInput.val(), notificationEditData.body)) {
            return true;
        }
    }

    return false;
}

$('#notificationCreateModal .btn-close, #notificationCreateModal #notificationCloseBtn').click(function (e) {
    if (checkNotificationCreateFormChanged()) {
        e.preventDefault();
        if (confirm('変更内容が保存されていません。\n本当に閉じてもよろしいですか？')) {
            $('#notificationCreateModal').modal('hide');
        }
    } else {
        $('#notificationCreateModal').modal('hide');
    }
});

// event after close modal
$('#notificationCreateModal').on('hidden.bs.modal', function () {
    localStorage.removeItem('notificationState');
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
    const isModalOpen = $('#notificationCreateModal').is(':visible');

    if (isModalOpen && checkNotificationCreateFormChanged()) {
        e.preventDefault();
        return '変更内容が保存されていません。\n本当にページを離れてもよろしいですか？';
    }
});

$('#notificationConfirmCreateBtn').click(function () {
    const formData = {};
    $('#notificationCreateForm :input').each(function () {
        const name = $(this).attr('name');
        if (name) {
            const type = $(this).attr('type');

            // Skip hidden input generated by Razor for checkboxes
            if (type === 'hidden' && $(`input[name="${name}"][type="checkbox"]`).length > 0) {
                return;
            }

            if (type === 'checkbox') {
                formData[name] = $(this).is(':checked');
            } else {
                formData[name] = $(this).val();
            }
        }
    });

    let notificationState = localStorage.getItem('notificationState');
    $.ajax({
        url: notificationState == 'create' ? '/Notification/Create' : '/Notification/Update',
        type: 'POST',
        data: formData,
        contentType: 'application/x-www-form-urlencoded',
        success: function (response) {
            // close modal
            $('#notificationCreateModal').modal('hide');
            // show modal success
            $('#notificationCreateResultModal').modal('show');
            $('#notificationCreateResultTitle').text('成功');
            // show success message
            $('#notificationCreateResultMessage').text(notificationState == 'create' ? '通知が作成されました。' : '通知が更新されました。');
            // reload datatable
            $('#notificationsTable').DataTable().ajax.reload();
            $('#notificationsTable').DataTable().draw();

        },
        error: function (xhr, status, error) {
            // close modal
            $('#notificationCreateModal').modal('hide');
            // show modal error
            $('#notificationCreateResultModal').modal('show');
            $('#notificationCreateResultTitle').text('エラー');
            // show error message
            $('#notificationCreateResultMessage').text(notificationState == 'create' ? '通知の作成に失敗しました。' : '通知の更新に失敗しました。');
        }
    });
});

$('#selectAllNotificationsCheckbox').change(function () {
    var isChecked = $(this).prop('checked');
    $('.notification-checkbox').prop('checked', isChecked);
    if (isChecked) {
        $('#deleteNotificationBtn').removeClass('d-none');
        $('#editNotificationBtn').addClass('d-none');
    } else {
        $('#deleteNotificationBtn').addClass('d-none');
    }
});

$('#deleteNotificationBtn').click(function () {
    $('#notificationDeleteConfirmModal').modal('show');
});

$('#notificationDeleteConfirmBtn').click(function () {
    $('#notificationDeleteConfirmModal').modal('hide');

    let selectedNotificationIds = [];
    $('.notification-checkbox:checked').each(function () {
        let ids = $(this).val().split(',');
        selectedNotificationIds = selectedNotificationIds.concat(ids);
    });

    $.ajax({
        url: '/Notification/Delete',
        type: 'POST',
        data: { ids: selectedNotificationIds },
        success: function (response) {
            $('#notificationCreateResultModal').modal('show');
            $('#notificationCreateResultTitle').text('成功');
            // show success message
            $('#notificationCreateResultMessage').text('通知が削除されました。');
            $('#notificationsTable').DataTable().ajax.reload();
            $('#notificationsTable').DataTable().draw();
        },
        error: function (xhr, status, error) {
            $('#notificationCreateResultModal').modal('show');
            $('#notificationCreateResultTitle').text('エラー');
            // show error message
            $('#notificationCreateResultMessage').text('通知の削除に失敗しました。');
        }
    });
});

$('#editNotificationBtn').click(function () {
    switchForm('edit', 'form');
    localStorage.setItem('notificationState', 'edit');
    // get selected notification id
    let selectedNotificationId = $('.notification-checkbox:checked').val();
    if (selectedNotificationId) {
        $('#notificationCreateModal').modal('show');
        clearAllContentOfCreateForm();
        clearAllCreateFormErrors();
        enableAllCreateFormElements();
        // show spinner
        let target = document.getElementById('notificationCreateModal');
        spinner.spin(target);
        // get notification data
        $.ajax({
            url: '/Notification/GetByIds',
            type: 'GET',
            data: { ids: selectedNotificationId },
            success: function (response) {
                // stop spinner
                spinner.stop();
                // set notification data
                $('#notificationCreateForm input[name="Title"]').val(response.title);
                $('#notificationCreateForm textarea[name="Body"]').val(response.body);
                notificationBodyEditor.setData(response.body);
                let isSpecial = response.isSpecial;
                if (isSpecial == true) {
                    $('#notificationCreateForm input[name="IsSpecial"]').prop('checked', true);
                };
                $('#notificationCreateForm #IsSpecial').trigger('change');
                $('#notificationCreateForm input[name="CourseTitle"]').val(response.courseTitle);
                if (response.scheduledTime) {
                    const localDate = new Date(response.scheduledTime);
                    const padZero = n => n.toString().padStart(2, '0');
                    const localString =
                        `${localDate.getFullYear()}-${padZero(localDate.getMonth() + 1)}-${padZero(localDate.getDate())}T${padZero(localDate.getHours())}:${padZero(localDate.getMinutes())}`;

                    $('#notificationCreateForm #ScheduledTimeInput').val(localString);
                    $('#notificationCreateForm input[name="ScheduledTime"]').val(response.scheduledTime);
                }
                $('#notificationCreateForm input[name="Id"]').val(response.id);
                if (response.isSent == true) {
                    disableAllCreateFormElements();
                }
                localStorage.setItem('notificationEditData', JSON.stringify(response));
            },
            error: function (xhr, status, error) {
                // stop spinner
                spinner.stop();
                // show error message
                $('#notificationCreateResultModal').modal('show');
                $('#notificationCreateResultTitle').text('エラー');
                $('#notificationCreateResultMessage').text('通知の取得に失敗しました。');
            }
        });
    }
});

var notificationBodyEditor;
import {
    ClassicEditor,
    AutoImage,
    Autosave,
    BlockQuote,
    Bold,
    Essentials,
    Heading,
    ImageBlock,
    ImageCaption,
    ImageInline,
    ImageInsert,
    ImageInsertViaUrl,
    ImageResize,
    ImageStyle,
    ImageTextAlternative,
    ImageToolbar,
    ImageUpload,
    Indent,
    IndentBlock,
    Italic,
    Link,
    LinkImage,
    List,
    ListProperties,
    Paragraph,
    SimpleUploadAdapter,
    Table,
    TableCaption,
    TableCellProperties,
    TableColumnResize,
    TableProperties,
    TableToolbar,
    TodoList,
    Underline
} from 'ckeditor5';

const LICENSE_KEY = 'GPL';

ClassicEditor
    .create(document.querySelector('#notificationCreateForm textarea[name="Body"]'), {
        licenseKey: LICENSE_KEY,
        toolbar: {
            items: [
                'undo',
                'redo',
                '|',
                'heading',
                '|',
                'bold',
                'italic',
                'underline',
                '|',
                'link',
                'insertImage',
                'insertTable',
                'blockQuote',
                '|',
                'bulletedList',
                'numberedList',
                'todoList',
                'outdent',
                'indent'
            ],
            shouldNotGroupWhenFull: false
        },
        plugins: [
            AutoImage,
            Autosave,
            BlockQuote,
            Bold,
            Essentials,
            Heading,
            ImageBlock,
            ImageCaption,
            ImageInline,
            ImageInsert,
            ImageInsertViaUrl,
            ImageResize,
            ImageStyle,
            ImageTextAlternative,
            ImageToolbar,
            ImageUpload,
            Indent,
            IndentBlock,
            Italic,
            Link,
            LinkImage,
            List,
            ListProperties,
            Paragraph,
            SimpleUploadAdapter,
            Table,
            TableCaption,
            TableCellProperties,
            TableColumnResize,
            TableProperties,
            TableToolbar,
            TodoList,
            Underline
        ],
        heading: {
            options: [
                {
                    model: 'paragraph',
                    title: 'Paragraph',
                    class: 'ck-heading_paragraph'
                },
                {
                    model: 'heading1',
                    view: 'h1',
                    title: 'Heading 1',
                    class: 'ck-heading_heading1'
                },
                {
                    model: 'heading2',
                    view: 'h2',
                    title: 'Heading 2',
                    class: 'ck-heading_heading2'
                },
                {
                    model: 'heading3',
                    view: 'h3',
                    title: 'Heading 3',
                    class: 'ck-heading_heading3'
                },
                {
                    model: 'heading4',
                    view: 'h4',
                    title: 'Heading 4',
                    class: 'ck-heading_heading4'
                },
                {
                    model: 'heading5',
                    view: 'h5',
                    title: 'Heading 5',
                    class: 'ck-heading_heading5'
                },
                {
                    model: 'heading6',
                    view: 'h6',
                    title: 'Heading 6',
                    class: 'ck-heading_heading6'
                }
            ]
        },
        image: {
            toolbar: [
                'toggleImageCaption',
                'imageTextAlternative',
                '|',
                'imageStyle:inline',
                'imageStyle:wrapText',
                'imageStyle:breakText',
                '|',
                'resizeImage'
            ]
        },
        link: {
            addTargetToExternalLinks: true,
            defaultProtocol: 'https://',
            decorators: {
                toggleDownloadable: {
                    mode: 'manual',
                    label: 'Downloadable',
                    attributes: {
                        download: 'file'
                    }
                }
            }
        },
        list: {
            properties: {
                styles: true,
                startIndex: true,
                reversed: true
            }
        },
        table: {
            contentToolbar: ['tableColumn', 'tableRow', 'mergeTableCells', 'tableProperties', 'tableCellProperties']
        },
        simpleUpload: {
            uploadUrl: '/Notification/UploadImage',
            headers: {
                'RequestVerificationToken': csrfToken
            }
        }
    })
    .then(editor => {
        notificationBodyEditor = editor;
        editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
            return new AzureUploadAdapter(loader);
        };
    })
    .catch(error => {
        console.error(error);
    });

class AzureUploadAdapter {
    constructor(loader) {
        this.loader = loader;
    }

    upload() {
        return this.loader.file
            .then(file => this.resizeImage(file))
            .then(resizedFile => this.sendRequest(resizedFile));
    }

    resizeImage(file) {
        return new Promise((resolve) => {
            const maxWidth = 800;
            const reader = new FileReader();
            reader.onload = function (event) {
                const img = new Image();
                img.onload = function () {
                    const canvas = document.createElement('canvas');
                    const scale = Math.min(1, maxWidth / img.width);
                    canvas.width = img.width * scale;
                    canvas.height = img.height * scale;
                    const ctx = canvas.getContext('2d');
                    ctx.drawImage(img, 0, 0, canvas.width, canvas.height);
                    canvas.toBlob(function (blob) {
                        const resizedFile = new File([blob], file.name, { type: blob.type });
                        resolve(resizedFile);
                    }, 'image/jpeg', 0.7); // nhẹ hơn
                };
                img.src = event.target.result;
            };
            reader.readAsDataURL(file);
        });
    }

    sendRequest(file) {
        const data = new FormData();
        data.append('file', file);

        return fetch('/Notification/UploadImage', {
            method: 'POST',
            headers: {
                'RequestVerificationToken': csrfToken
            },
            body: data
        })
            .then(response => {
                if (!response.ok) throw new Error('Upload failed');
                return response.json();
            })
            .then(result => {
                if (result.error) throw new Error(result.error.message);
                return { default: result.url };
            });
    }

    abort() {
        // optional
    }
}