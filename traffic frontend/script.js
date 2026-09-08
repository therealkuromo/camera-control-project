// ============================================
// API
// ============================================

const API_URL =
    "https://localhost:7163/api/Traffic";


// ============================================
// DOM Elements
// ============================================

const endpointButtons =
    document.querySelectorAll(
        ".endpoint-button"
    );


const resultContainer =
    document.getElementById(
        "resultContainer"
    );


const endpointTitle =
    document.getElementById(
        "endpointTitle"
    );


const endpointDescription =
    document.getElementById(
        "endpointDescription"
    );


const recordCount =
    document.getElementById(
        "recordCount"
    );


const totalEvents =
    document.getElementById(
        "totalEvents"
    );


const totalViolations =
    document.getElementById(
        "totalViolations"
    );


const violationPercentage =
    document.getElementById(
        "violationPercentage"
    );


const loadingMessage =
    document.getElementById(
        "loadingMessage"
    );


const errorMessage =
    document.getElementById(
        "errorMessage"
    );


const refreshButton =
    document.getElementById(
        "refreshButton"
    );


// ============================================
// Endpoint Configuration
// ============================================

const endpoints = {

    traffic: {
        url: "",
        title: "All Traffic",
        description:
            "All traffic events stored in the database."
    },


    violations: {
        url: "/violations",
        title: "Speed Violations",
        description:
            "Vehicles that exceeded the allowed speed."
    },


    cameras: {
        url: "/cameras",
        title: "Violations By Camera",
        description:
            "Number of violations detected by each camera."
    },


    "last-violations": {
        url: "/last-violations",
        title: "Last Violation By Plate",
        description:
            "The latest violation recorded for each plate."
    },


    "frequent-violators": {
        url: "/frequent-violators",
        title: "Frequent Violators",
        description:
            "Plates with more than five violations."
    },


    "cameras-without-violations": {
        url: "/cameras-without-violations",
        title: "Cameras Without Violations",
        description:
            "Cameras that have not detected any violations."
    },


    "max-speed": {
        url: "/max-speed",
        title: "Maximum Speed",
        description:
            "Maximum recorded speed for each camera."
    },


    "top-violators": {
        url: "/top-violators",
        title: "Top 3 Violators",
        description:
            "Three plates with the highest number of violations."
    },


    "violation-percentage": {
        url: "/violation-percentage",
        title: "Violation Percentage",
        description:
            "Percentage of traffic events that are violations."
    }

};


// ============================================
// Load Endpoint
// ============================================

async function loadEndpoint(
    endpointName
) {

    const endpoint =
        endpoints[endpointName];


    if (!endpoint) {
        return;
    }


    showLoading();


    endpointTitle.textContent =
        endpoint.title;


    endpointDescription.textContent =
        endpoint.description;


    try {

        const response =
            await fetch(
                API_URL + endpoint.url
            );


        if (!response.ok) {

            throw new Error(
                `HTTP Error: ${response.status}`
            );

        }


        const data =
            await response.json();


        displayData(
            data,
            endpointName
        );


        hideLoading();

    }

    catch (error) {

        console.error(error);

        showError(
            "Could not connect to the API."
        );

        hideLoading();
    }
}


// ============================================
// Display Data
// ============================================

function displayData(
    data,
    endpointName
) {

    resultContainer.innerHTML = "";


    // ----------------------------------------
    // Violation Percentage
    // ----------------------------------------

    if (
        endpointName ===
        "violation-percentage"
    ) {

        displayPercentage(data);

        return;
    }


    // ----------------------------------------
    // Array
    // ----------------------------------------

    if (Array.isArray(data)) {

        recordCount.textContent =
            `${data.length} records`;


        if (data.length === 0) {

            resultContainer.innerHTML =
                `<div class="empty">
                    No data found.
                 </div>`;

            return;
        }


        createTable(data);

        return;
    }


    // ----------------------------------------
    // Object
    // ----------------------------------------

    recordCount.textContent =
        "1 result";


    createTable(
        [data]
    );
}


// ============================================
// Create Table
// ============================================

function createTable(data) {

    const table =
        document.createElement(
            "table"
        );


    const thead =
        document.createElement(
            "thead"
        );


    const tbody =
        document.createElement(
            "tbody"
        );


    // ----------------------------------------
    // Header
    // ----------------------------------------

    const headerRow =
        document.createElement(
            "tr"
        );


    const columns =
        Object.keys(data[0]);


    columns.forEach(column => {

        const th =
            document.createElement(
                "th"
            );


        th.textContent =
            formatColumnName(
                column
            );


        headerRow.appendChild(th);

    });


    thead.appendChild(
        headerRow
    );


    // ----------------------------------------
    // Rows
    // ----------------------------------------

    data.forEach(item => {

        const row =
            document.createElement(
                "tr"
            );


        columns.forEach(column => {

            const td =
                document.createElement(
                    "td"
                );


            td.textContent =
                formatValue(
                    item[column]
                );


            row.appendChild(td);

        });


        tbody.appendChild(row);

    });


    table.appendChild(thead);

    table.appendChild(tbody);

    resultContainer.appendChild(table);
}


// ============================================
// Format Column Name
// ============================================

function formatColumnName(
    column
) {

    return column
        .replace(
            /([A-Z])/g,
            " $1"
        )
        .replace(
            /^./,
            character =>
                character.toUpperCase()
        );
}


// ============================================
// Format Value
// ============================================

function formatValue(value) {

    if (value === null ||
        value === undefined) {

        return "-";
    }


    if (
        typeof value === "string" &&
        value.includes("T")
    ) {

        const date =
            new Date(value);


        if (!isNaN(date)) {

            return date.toLocaleString();

        }
    }


    return value;
}


// ============================================
// Percentage
// ============================================

function displayPercentage(
    data
) {

    recordCount.textContent =
        "Statistics";


    resultContainer.innerHTML = `

        <div class="statistics">

            <div class="stat-card">

                <h3>Total Events</h3>

                <p>
                    ${data.totalEvents}
                </p>

            </div>


            <div class="stat-card">

                <h3>Violations</h3>

                <p>
                    ${data.violations}
                </p>

            </div>


            <div class="stat-card">

                <h3>Violation Rate</h3>

                <p>
                    ${data.percentage}%
                </p>

            </div>

        </div>

    `;
}


// ============================================
// Dashboard Statistics
// ============================================

async function loadStatistics() {

    try {

        const [
            trafficResponse,
            violationsResponse,
            percentageResponse
        ] = await Promise.all([

            fetch(API_URL),

            fetch(
                `${API_URL}/violations`
            ),

            fetch(
                `${API_URL}/violation-percentage`
            )

        ]);


        if (
            !trafficResponse.ok ||
            !violationsResponse.ok ||
            !percentageResponse.ok
        ) {

            throw new Error(
                "Failed to load statistics."
            );

        }


        const traffic =
            await trafficResponse.json();


        const violations =
            await violationsResponse.json();


        const percentage =
            await percentageResponse.json();


        totalEvents.textContent =
            traffic.length;


        totalViolations.textContent =
            violations.length;


        violationPercentage.textContent =
            `${percentage.percentage}%`;

    }

    catch (error) {

        console.error(error);

    }
}


// ============================================
// Loading
// ============================================

function showLoading() {

    loadingMessage.classList.remove(
        "hidden"
    );

    errorMessage.classList.add(
        "hidden"
    );
}


function hideLoading() {

    loadingMessage.classList.add(
        "hidden"
    );
}


// ============================================
// Error
// ============================================

function showError(message) {

    errorMessage.textContent =
        message;

    errorMessage.classList.remove(
        "hidden"
    );
}


// ============================================
// Endpoint Buttons
// ============================================

endpointButtons.forEach(button => {

    button.addEventListener(
        "click",
        () => {

            endpointButtons.forEach(
                item =>
                    item.classList.remove(
                        "active"
                    )
            );


            button.classList.add(
                "active"
            );


            const endpointName =
                button.dataset.endpoint;


            loadEndpoint(
                endpointName
            );

        }
    );

});


// ============================================
// Refresh
// ============================================

refreshButton.addEventListener(
    "click",
    () => {

        const activeButton =
            document.querySelector(
                ".endpoint-button.active"
            );


        loadEndpoint(
            activeButton.dataset.endpoint
        );


        loadStatistics();

    }
);


// ============================================
// Initial Load
// ============================================

loadStatistics();

loadEndpoint(
    "traffic"
);

