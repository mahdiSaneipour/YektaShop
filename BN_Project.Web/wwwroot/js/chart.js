const charts = document.querySelectorAll(".box-chart");

charts.forEach(function (element) {
    const ctx = element.querySelector('#myChart');
    const inputs = element.querySelectorAll(".sell-data-chart-10-days");

    const labels = [];
    const datas = [];

    inputs.forEach(function (element) {
        labels.push(element.getAttribute("title"));
    })

    inputs.forEach(function (element) {
        datas.push(element.getAttribute("data"));
    })

    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                label: "",
                data: datas,
            }]
        }
    });
})

