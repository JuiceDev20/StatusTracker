$(function() {
    "use strict";

    $('.knob2').knob({
        'format' : function (value) {
            return value + '%';
         }
    });

    // progress bars
    $('.progress .progress-bar').progressbar({
            display_text: 'none'
    });
    $('.sparkline-pie').sparkline('html', {
        type: 'pie',
        offset: 90,
        width: '147px',
        height: '147px',
        sliceColors: ['#02b5b2', '#445771', '#ffcd55']
    })
    $('#sparkline-compositeline').sparkline('html', {
        fillColor: false,
        lineColor: '#445771',
        width: '200px',
        height: '30px',
        lineWidth: '1',
    });
    $('#sparkline-compositeline').sparkline([4, 1, 5, 7, 9, 9, 8, 7, 6, 6, 4, 7, 8, 4, 3, 2, 2, 5, 6, 7], {
        composite: true,
        fillColor: false,
        lineColor: '#02b5b2',
        lineWidth: '1',
        chartRangeMin: 0,
        chartRangeMax: 10
    });
    $('#sparkline-compositeline').sparkline([6, 4, 7, 8, 4, 3, 2, 2, 5, 6, 7, 4, 1, 5, 7, 9, 9, 8, 7, 6], {
        composite: true,
        fillColor: false,
        lineColor: '#ffcd55',
        lineWidth: '1',
        chartRangeMin: 0,
        chartRangeMax: 10
    });

    // =================    
    $('.sparkbar').sparkline('html', { type: 'bar' });

    // notification popup
    toastr.options.closeButton = true;
    toastr.options.positionClass = 'toast-bottom-right';    
    toastr['success']('Hello and welcome to Status Tracker, a unique admin Template.');
});

// top products
var dataStackedBar = {
    labels: ['Q1','Q2','Q3','Q4','Q5','Q6'],
    series: [
        [2350,3205,4520,2351,5632,3205],
        [2541,2583,1592,2674,2323,2583],
        [1212,5214,2325,4235,2519,5214],
    ]
};
new Chartist.Bar('#Salary_Statistics', dataStackedBar, {
    height: "230px",
    stackBars: true,
    axisX: {
        showGrid: false
    },
    axisY: {
        labelInterpolationFnc: function(value) {
            return (value / 1000) + 'k';
        }
    },
    plugins: [
        Chartist.plugins.tooltip({
            appendToBody: true
        }),
        Chartist.plugins.legend({
            legendNames: ['Developer', 'Marketing', 'Sales']
        })
    ]
}).on('draw', function(data) {
        if (data.type === 'bar') {
            data.element.attr({
                style: 'stroke-width: 30px'
            });
        }
});


// multiple chart
var dataMultiple = {
    labels: ['9/28', '10/5', '10/12', '10/19', '10/26', '11/2', '11/9', '11/16', '11/23', '11/30', '12/7', '12/14', '12/21',],
    series: [{
        name: 'Challenges',
        data: [0, 20, 20, 20, 20, 20, 20, 0, 0, 0, 0, 0, 0],
    }, {
        name: 'Blog',
        data: [0, 0, 0, 0, 40, 40, 40, 8, 4, 4, 4, 4, 4],            
    },{
        name: 'Status Tracker',
        data: [0, 0, 0, 0, 0, 60, 60, 60, 60, 2, 2, 2, 2],            
    },{
        name: 'Address Book',
        data: [0, 0, 0, 0, 0, 0, 0, 40, 4, 0, 0, 0, 0],            
    },{
        name: 'Financial Portal',
        data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 40, 40, 20],            
    }]
};
options = {
    lineSmooth: true,
    height: "330px",
    low: 0,
    high: 'auto',
    series: {
        'Design': {
            showPoint: true,                
        },
    },

    axisY: {
        labelInterpolationFnc: function(value) {
            return (value);
        }
    },
    
    options: {
        responsive: true,
        legend: true
    },

    plugins: [
        Chartist.plugins.legend({
            legendNames: ['Challenges', 'Blog', 'Status Tracker', 'Address Book', 'Financial Portal']
        })
    ]
};
new Chartist.Line('#workSchedule', dataMultiple, options);
