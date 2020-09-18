document.writeln("<script type='text/javascript' src='config.js'></script>")

function ChangeDiscipline(select) 
{
    var table = document.getElementById('studentsTable');
    var groupsSelect = document.getElementById('Groups');
    groupsSelect.selectedIndex = 0;
    for (var i = 1; i < groupsSelect.options.length; i++)
    {
        groupsSelect.remove(i);
    }
    table.innerHTML = '';

    var jwt = GetJWT();
    var selectedDiscipline = select.options[select.selectedIndex].value;
    fetch(api + 'Groups/ByDiscipline/' + selectedDiscipline,
        {
            method: 'GET',
            headers:
            {
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + jwt,
                Accept: 'applicatin/json'
            }
        })
        .then(res => res.json())
        .then(data => setUpGroups(data))
        .catch(err => console.log(err));
}

function setUpGroups(groupsJson)
{
    var groupsSelect = document.getElementById("Groups");

    for (var i = 0; i < groupsJson.length; i++) {
        var option = document.createElement("option");
        var group = groupsJson[i];
        option.value = group.id;
        option.text = group.name;
        groupsSelect.appendChild(option);
    }
}

function ChangeGroup(select)
{
    var jwt = GetJWT();
    var selectedGroup = select.options[select.selectedIndex].value;
    fetch(api + 'Students/ByGroup/' + selectedGroup,
        {
            method: 'GET',
            headers:
            {
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + jwt,
                Accept: 'application/json'
            }
        })
        .then(res => res.json())
        .then(data => getClasses(data))
        .catch(err => console.log(err));
}

function getClasses(students)
{
    var jwt = GetJWT();
    var groupsSelect = document.getElementById("Groups");
    var disciplineSelect = document.getElementById("Disciplines");

    var selectedGroup = groupsSelect.options[groupsSelect.selectedIndex].value;
    var selectedDiscipline = disciplineSelect.options[disciplineSelect.selectedIndex].value;

    fetch(api + 'Classes/ByGroupDiscipline/' + selectedGroup + '/' + selectedDiscipline,
        {
            method: 'GET',
            headers:
            {
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + jwt,
                Accept: 'application/json'
            }
        })
        .then(res => res.json())
        .then(data => setUpStudents(students, data));
}

function setUpStudents(students, classes)
{
    var table = document.getElementById('studentsTable');
    var tbody = document.createElement('tbody');
    var classIds = new Array();

    var tr = document.createElement("tr");
    var td = document.createElement("td");
    td.classList = 'row';

    tr.appendChild(document.createElement("td"));
    tr.appendChild(document.createElement("td"));

    for (var i = 0; i < classes.length; i++)
    {
        var date = new Date(classes[i].date);
        td = document.createElement("td");
        td.innerText = date.getDate() + '.' + (date.getMonth() + 2) + '.' + date.getFullYear();
        td.classList += ' text-center';
        tr.appendChild(td);

        classIds.push(classes[i].id);
    }

    // Table Today date header
    var dateTd = document.createElement("td");
    td.classList = 'row';
    var today = new Date();
    dateTd.innerText = today.getDate() + '.' + today.getMonth() + '.' + today.getFullYear();
    tr.appendChild(dateTd);
    tbody.appendChild(tr);

    fetch(api + 'Students/ByClasses/',
        {
            method: 'POST',
            headers:
            {
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + GetJWT(),
                Accept: 'application/json'
            },
            body: JSON.stringify(classIds)
        })
        .then(res => res.json())
        .then(data => MarkVisited(table, tbody, students, data))
        .catch(err => console.log(err));
}

function Save()
{
    var table = document.getElementById('studentsTable');
    var ids = JSON.parse(window.localStorage.getItem('Ids'));
    var visited = new Array();

    for (var i = 1; i < table.rows.length; i++)
    {
        var input = document.getElementById('student' + i);
        if (input.value == '+')
        {
            visited.push(ids[i - 1]);
        }
    }

    var cells = table.rows[0].cells;
    var testCells = table.rows[0].cells[table.rows[0].cells.length - 1];
    var date = table.rows[0].cells[table.rows[0].cells.length - 1].innerText;
    var disciplineSelect = document.getElementById('Disciplines');
    var disciplineId = disciplineSelect.options[disciplineSelect.selectedIndex].value;
    var groupSelect = document.getElementById('Groups');
    var groupId = groupSelect.options[groupSelect.selectedIndex].value;

    var newClass = new Class(date, disciplineId, groupId, visited);
    var jwt = GetJWT();
    var classJson = JSON.stringify(newClass);
    fetch(api + 'classes/Add',
        {
            method: 'POST',
            mode: 'cors',
            headers:
            {
                'Content-Type': 'application/json',
                Authorization: 'Bearer ' + jwt,
                Accept: 'application/json',
            },
            body: classJson
        })
        .then(res => SavePopUp(res));
}

function SavePopUp(response)
{

}

function MarkVisited(table, tbody, students, visited)
{
    var ids = new Array();
    for (var i = 0; i < students.length; i++)
    {
        ids.push(students[i].id);

        var tr = document.createElement("tr");
        var td = document.createElement("td");

        td.scope = 'row';
        td.innerText = i + 1;
        tr.appendChild(td);

        td = document.createElement("td");
        td.innerText = students[i].lastName + ' ' + students[i].firstName + ' ' + students[i].middleName;
        tr.appendChild(td);

        for (var j = 0; j < visited.length; j++)
        {
            td = document.createElement("td");

            for (var k = 0; k < visited[j].length; k++)
            {
                if (students[i].id == visited[j][k].id)
                {
                    td.innerText = '+';
                    td.classList = 'text-center';
                    break;
                }
            }
            if (td.innerText != '+') td.innerText = ' ';
            tr.appendChild(td);
        }

        td = document.createElement("td");
        var input = document.createElement("input");
        input.type = 'text';
        input.id = 'student' + (i + 1).toString();
        input.maxLength = 1;
        input.size = 3;
        td.appendChild(input);
        tr.appendChild(td);

        tbody.appendChild(tr);
    }
    window.localStorage.setItem('Ids', JSON.stringify(ids));
    table.appendChild(tbody);
}

function GetJWT()
{
    var match = document.cookie.match(new RegExp('(^| )' + 'Jwt' + '=([^;]+)'));
    if (match) {
        return match[2];
    }
    return '';
}

class Class
{
    Date;
    DisciplineId;
    GroupId;
    VisitedStudents = new Array();

    constructor(date, disciplineId, groupId, visitedStudents)
    {
        this.Date = date;
        this.DisciplineId = parseInt(disciplineId);
        this.GroupId = parseInt(groupId);
        this.VisitedStudents = visitedStudents;
    }
}