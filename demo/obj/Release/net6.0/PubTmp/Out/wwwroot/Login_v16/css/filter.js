document.addEventListener("DOMContentLoaded", () => {
    populateCityFilter();
});

function populateCityFilter() {
    const cityFilter = document.getElementById("city-filter");
    const cities = new Set();

    // Collect unique city names from the table
    document.querySelectorAll("td[data-city]").forEach(cell => {
        cities.add(cell.getAttribute("data-city"));
    });

    // Add each unique city as an option in the dropdown
    cities.forEach(city => {
        const option = document.createElement("option");
        option.value = city;
        option.textContent = city;
        cityFilter.appendChild(option);
    });
}
function filter_rows() {
    const filterValue = document.getElementById("city-filter").value;
    const rows = document.querySelectorAll("table tbody tr");

    rows.forEach(row => {
        const city = row.querySelector('td[data-city]').getAttribute('data-city');

        if (filterValue === "all" || city === filterValue) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}
document.addEventListener("DOMContentLoaded", () => {
    populateWardFilter();
});

function populateWardFilter() {
    const wardFilter = document.getElementById("ward-filter");
    const wards = new Set();

    // Collect unique wards from the table
    document.querySelectorAll("td[data-ward]").forEach(cell => {
        const ward = cell.getAttribute("data-ward");
        if (ward) {
            wards.add(ward);
        }
    });

    // Add each unique ward as an option in the dropdown
    wards.forEach(ward => {
        const option = document.createElement("option");
        option.value = ward;
        option.textContent = ward; // Display the Wardno-Wardname in the dropdown
        wardFilter.appendChild(option);
    });
}

function filter_wards() {
    const filterValue = document.getElementById("ward-filter").value;
    const rows = document.querySelectorAll("table tbody tr");

    rows.forEach(row => {
        const ward = row.querySelector("td[data-ward]").getAttribute("data-ward");

        if (filterValue === "all" || ward === filterValue) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}

