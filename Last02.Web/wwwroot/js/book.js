let bookCodeResponse = null;
$(function () {
    $('#bookCodeTable').DataTable({
        processing: true,
        serverSide: true,
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
        scrollY: 200,
        deferRender: true,
        ajax: {
            url: '/Book/Search',
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
            { data: 'bookInstanceCode' },
            {
                data: 'isUsed',
                render: function (data, type, row) {
                    return data ? '使用済み' : '未使用';
                },
                className: 'text-center'
            },
        ],
    });
});

$('#bookOpenCodeGenerateModalBtn').on('click', function () {
    $('#numberCode').val('');
    $('#numberCodeInvalidFeedback').html('');
    $('#numberCode').removeClass('is-invalid');
    $('#bookCodeGenerateModal').modal('show');
});

$('#bookCodeGenerateCloseBtn').on('click', function () {
    $('#bookCodeGenerateModal').modal('hide');
});

$('#bookCodeGenerateBtn').on('click', function () {
    const numberCode = $('#numberCode').val();
    if (numberCode < 1 || numberCode > 1000) {
        $('#numberCodeInvalidFeedback').html('1以上1000以下で入力してください。');
        $('#numberCode').addClass('is-invalid');
        return;
    }
    $('#numberCode').removeClass('is-invalid');

    $.ajax({
        url: '/Book/GenerateCodes',
        type: 'POST',
        data: { count: numberCode },
        success: function (response) {
            bookCodeResponse = response;
            $('#bookCodeTable').DataTable().ajax.reload();
            $('#bookCodeGenerateModal').modal('hide');
            $('#bookCodeResultTitle').html('作成完了');
            $('#bookCodeResultContent').html("書籍コードの作成に成功しました。\r\n 作成された書籍コードが含まれるファイルをダウンロードするには、ダイアログを閉じてください。");
            $('#bookCodeResultModal').modal('show');
        },
        error: function (error) {
            $('#bookCodeGenerateModal').modal('hide');
            $('#bookCodeResultTitle').html('エラー');
            $('#bookCodeResultContent').html("書籍コードの作成に失敗しました");
            $('#bookCodeResultModal').modal('show');
        }
    });
});

$('#bookCodeResultModal').on('hide.bs.modal', function () {
    if (bookCodeResponse) {
        const blob = new Blob([bookCodeResponse], { type: 'text/csv' });
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'book_codes_' + new Date().toISOString().replace(/\D/g, '') + '.csv';
        a.click();
        URL.revokeObjectURL(url);
        bookCodeResponse = null;
    }
});


$('#bookOpenCodeDownloadModalBtn').on('click', function () {
    window.location.href = '/Book/DownloadAllCodes';
});
