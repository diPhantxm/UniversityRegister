

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

    groupsSelect.innerHTML = '';
    var option = document.createElement("option");
    option.innerText = "Выберите Группу";
    groupsSelect.appendChild(option);

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

    for (var i = 0; i < groupsJson.length; i++)
    {
        var option = document.createElement("option");
        var group = groupsJson[i];
        option.value = group.id;
        option.text = group.name;
        groupsSelect.appendChild(option);
    }
}

function ChangeGroup(select)
{
    if (select.options[select.selectedIndex].value == "Выберите Группу")
    {
        document.getElementById("studentsTable").innerHTML = '';
        return;
    }

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
    window.localStorage.setItem('Classes', JSON.stringify(classes));
    window.localStorage.setItem('Students', JSON.stringify(students));

    var table = document.getElementById('studentsTable');
    table.innerHTML = '';
    var tbody = document.createElement('tbody');
    var classIds = new Array();

    var tr = document.createElement("tr");
    var td = document.createElement("td");
    td.classList.add('row');
    td.classList.add('text-center');

    tr.appendChild(document.createElement("td"));
    tr.appendChild(document.createElement("td"));

    for (var i = classes.length - 1; i >= 0 && i >= classes.length - 5; i--)
    {
        var date = new Date(classes[i].date);
        td = document.createElement("td");
        td.classList.add('text-center'); 
        td.innerText = date.getDate() + '.' + (date.getMonth() + 1) + '.' + date.getFullYear();
        tr.appendChild(td);

        classIds.push(classes[i].id);
    }

    // Table Today date header
    var dateTd = document.createElement("td");
    dateTd.classList.add('text-center');
    var today = new Date();
    dateTd.innerText = today.getDate() + '.' + (today.getMonth() + 1) + '.' + today.getFullYear();
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
        var columnsCount = table.rows[i].cells.length;
        var plusTd = table.rows[i].cells[columnsCount - 1];
        if (plusTd.innerText == '+')
        {
            visited.push(ids[i - 1]);
        }
    }

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
    if (!response.ok) return;

    var table = document.getElementById('studentsTable');
    var lastColIndex = table.rows[0].cells.length - 1;
    for (var i = 1; i < table.rows.length; i++)
    {
        var row = table.rows[i];

        row.cells[lastColIndex].classList.add('text-center');
        row.cells[lastColIndex].classList.add('align-middle');

        if (row.cells[lastColIndex].innerText == '+') continue;

        if (row.cells[lastColIndex].innerText == '')
        {
            row.cells[lastColIndex].innerHTML = '';
            row.cells[lastColIndex].innerText = 'H';
            row.cells[lastColIndex].style.backgroundColor = '#ea907a';
        }
    }

    var dateTr = document.createElement('td');
    dateTr.classList.add('text-center');
    var today = new Date();
    dateTr.innerText = today.getDate() + '.' + (today.getMonth() + 1) + '.' + today.getFullYear();
    table.rows[0].appendChild(dateTr);

    var ids = JSON.parse(window.localStorage.getItem('Ids'));

    if (table.rows[0].cells.length > 7)
    {
        table.rows[0].removeChild(table.rows[0].cells[2]); // Remove date of first class
    }

    for (var i = 1; i < table.rows.length; i++)
    {
        if (table.rows[i].cells.length > 7)
        {
            var firstClass = table.rows[i].cells[2];
            table.rows[i].removeChild(firstClass);
        }

        var plusBtn = CreatePlusButton(ids[i - 1]);
        var td = document.createElement('td');
        td.classList.add('text-center');
        td.appendChild(plusBtn);
        table.rows[i].appendChild(td);
    }
}

function MarkVisited(table, tbody, students, visited)
{
    window.localStorage.setItem('Visited', JSON.stringify(visited));

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
        var a = document.createElement("a");
        var popup = CreatePopup(students[i]);
        a.href = "#" + popup.id;
        a.rel = "modal:open";
        a.innerText = students[i].lastName + ' ' + students[i].firstName;
        td.appendChild(a);
        tr.appendChild(td);

        for (var j = visited.length - 1; j >= 0 && j >= visited.length - 5; j--)
        {
            td = document.createElement("td");
            td.classList.add('text-center');
            td.classList.add('align-middle');

            for (var k = 0; k < visited[j].length; k++)
            {
                if (students[i].id == visited[j][k].id)
                {
                    td.innerText = '+';
                    td.style.backgroundColor = 'rgb(209, 234, 163)';
                    break;
                }
            }
            if (td.innerText != '+')
            {
                td.innerText = 'H';
                td.style.backgroundColor = '#ea907a';
            }
            tr.appendChild(td);
        }

        td = document.createElement("td");
        td.classList.add('text-center');

        var input = CreatePlusButton(students[i].id);
        td.appendChild(input);
        tr.appendChild(td);

        tbody.appendChild(tr);
    }
    window.localStorage.setItem('Ids', JSON.stringify(ids));
    table.appendChild(tbody);

    document.getElementById('SaveBtn').style.visibility = "visible";
}

function CreatePlusButton(id)
{
    var input = document.createElement("input");
    input.type = 'button';
    input.value = '+'
    input.id = id.toString();
    input.maxLength = 1;
    input.size = 3;
    input.classList.add('btn');
    input.classList.add('btn-light');
    input.addEventListener("click", AddVisited);

    return input;
}

function GetJWT()
{
    var match = document.cookie.match(new RegExp('(^| )' + 'Jwt' + '=([^;]+)'));
    if (match) {
        return match[2];
    }
    return '';
}

function AddVisited(btn)
{
    var input = document.getElementById(btn.currentTarget.id);
    var td = input.parentElement;
    td.removeChild(input);
    td.innerText = '+';
    td.classList.add('text-center');
    td.style.backgroundColor = 'rgb(209, 234, 163)';
}

function CreatePopup(student)
{
    var div = document.createElement("div");
    div.classList.add("modal");
    div.style.height = "20%";
    div.id = "popup" + student.id;

    var visited = JSON.parse(window.localStorage.getItem("Visited"));
    var classes = JSON.parse(window.localStorage.getItem("Classes"));
    var classesCount = classes.length;

    var visits = CalcVisits(visited, student.id);
    var percantage = visits / classesCount;
    if (classesCount == 0) percantage = 1;

    var p = document.createElement("p");
    p.innerHTML = `<b>${student.id}</b>
        <p>${student.firstName} ${student.lastName}</p>
        <p>Посещаемость по предмету: <span class="badge badge-secondary" style="Background-Color: rgb(${ (1 - percantage) * 255}, ${percantage * 255}, 0)">${Number((percantage * 100).toFixed(1))}%</span></p>
        <p>Email: ${ student.mail }</p>`;

    var a = document.createElement("a");
    a.href = "#";
    a.rel = "modal:close";
    a.innerText = "Закрыть";

    div.appendChild(p);
    div.appendChild(a);

    document.body.appendChild(div);

    return div;
}

function CalcVisits(visited, id)
{
    var result = 0;
    for (var i = 0; i < visited.length; i++)
    {
        for (var j = 0; j < visited[i].length; j++)
        {
            if (visited[i][j].id == id) result++;
        }
    }

    return result;
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

function logout()
{
    $.get("Home/Logout");
    window.location.href = 'Home/';
}