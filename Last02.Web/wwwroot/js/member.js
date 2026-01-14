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

let memberConfirmModal;

$(function () {
    const totalColumns = $('#memberTable thead th').length;
    const sortableColumns = [0, 1, 2];
    const unsortableColumns = [...Array(totalColumns).keys()].filter(i => !sortableColumns.includes(i));

    new DataTable('#memberTable', {
        processing: true,
        serverSide: true,
        scrollY: 200,
        deferRender: true,
        ordering: true,
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
            url: '/Member/Search',
            type: 'GET',
            dataType: "json",
            dataSrc: {
                data: 'data',
                draw: 'request',
                recordsTotal: 'totalRecords',
                recordsFiltered: 'recordsFiltered'
            },
            data: function (d) {
                const orders = d.order.map(o => ({
                    column: d.columns[o.column].data,  
                    dir: o.dir                        
                }));

                return {
                    keyword: d.search.value,
                    page: (d.start / d.length) + 1,
                    size: d.length,
                    orders: JSON.stringify(orders)
                };
            }
        },
        columns: [
            { data: 'id' },
            { data: 'fullName' },
            { data: 'email' },
            {
                data: 'gender',
                render: function (data, type, row) {
                    return data == 0 ? 'Male' : 'Female';
                },
                className: 'text-center'
            },
            {
                data: 'dob',
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
            { data: 'nationaity' },
            { data: 'courseSelection' },
            {
                data: 'createdAt',
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
                data: 'updatedAt',
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
                data: 'isActive',
                render: function (data, type, row) {
                    if (row.isActive) {
                        return `<div class="d-flex justify-content-center">
                                    <button type="button" class="btn btn-danger btn-sm member-btn-width btn-deactivate" data-id="${row.id}">Deactivate</button>
                                </div>`;
                    } else {
                        return `<div class="d-flex justify-content-center">
                                    <button type="button" class="btn btn-info btn-sm member-btn-width btn-activate" data-id="${row.id}">Activate</button>
                                </div>`;
                    }
                }
            },
        ],
        columnDefs: [
            {
                targets: unsortableColumns,
                orderable: false
            },
            {
                targets: [0, 1, 2],
                orderable: true
            },
            {
                targets: '_all',
                className: 'member-td-max-width text-wrap dt-center'
            },
        ],
        initComplete: function () {
            const table = this.api();
            const totalColumns = table.columns().count();
            const sortableColumns = [0, 1, 2];
            const unsortable = [...Array(totalColumns).keys()].filter(i => !sortableColumns.includes(i));
            table.columns(unsortable).every(function () {
                this.orderable(false);
            });
            $('#dt-search-0')
                .attr('placeholder', 'Please enter FullName or Email')
                .css({ width: '250px' });
        },
        //event after draw
        drawCallback: function () {
            $('.member-checkbox').on('change', function () {
                let checkedCount = $('#memberTable tbody').find('.member-checkbox:checked').length;
                let totalCount = $('#memberTable tbody').find('.member-checkbox').length;
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

            $('#memberTable tbody tr td').not('.select-checkbox').on('click', function (e) {
                $('.member-checkbox').prop('checked', false);
                const checkbox = $(this).parent().find('.member-checkbox');
                checkbox.prop('checked', !checkbox.prop('checked')).trigger('change');
            });

            $('#editCourseBtn').addClass('d-none');
            $('#deleteCourseBtn').addClass('d-none');
            $('#selectAllCoursesCheckbox').prop('checked', false);
        }
    });

    var memberModalConfirm = document.getElementById('member-modal-confirm');
    if (memberModalConfirm !== null) {
        memberConfirmModal = new bootstrap.Modal(document.getElementById('member-modal-confirm'), {
            keyboard: false
        })
    }

    $('#memberTable tbody').on('click', '.btn-activate', function () {
        const id = $(this).data('id');
        activateMember(id);
    });

    $('#memberTable tbody').on('click', '.btn-deactivate', function () {
        const id = $(this).data('id');
        deactivateMember(id);
    });

    var btnExport = document.getElementById("btnExport");
    if (btnExport != null) {
        document.getElementById("btnExport").addEventListener("click", function () {
            let keyword = document.getElementById("dt-search-0").value;
            window.location.href = "/Member/ExportToExcel?keyword=" + keyword;
        });
    }
});

function showModalConfirm(title, message) {
    document.getElementById("member-modal-confirm-title").textContent = title;
    document.getElementById("member-modal-confirm-body").textContent = message;
    memberConfirmModal.show();
}

function hideModalConfirm() {
    memberConfirmModal.hide();
}

function activateMember(id) {
    // store id to local storage
    localStorage.setItem('memberId', id);
    localStorage.setItem('action', 'activate');
    showModalConfirm('Active Member', 'Are you sure you want to active this member?');
}

function deactivateMember(id) {
    // store id to local storage
    localStorage.setItem('memberId', id);
    localStorage.setItem('action', 'deactivate');
    showModalConfirm('DeActive Member', 'Are you sure you want to deActive this member?');
}

function doActivateMember() {
    hideModalConfirm();
    // call api to activate member
    let url = '/Member/ActivateMember';
    let formData = new FormData();
    formData.append('id', localStorage.getItem('memberId'));
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            // reload page
            location.reload();
        }
    });
}

function doDeactivateMember() {
    hideModalConfirm();
    // call api to deactivate member
    let url = '/Member/DeactivateMember';
    let formData = new FormData();
    formData.append('id', localStorage.getItem('memberId'));
    $.ajax({
        url: url,
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        success: function (response) {
            // reload page
            location.reload();
        }
    });
}

$('#member-modal-confirm-ok').on('click', function () {
    let action = localStorage.getItem('action');
    if (action == 'activate') {
        doActivateMember();
    } else if (action == 'deactivate') {
        doDeactivateMember();
    }
});

