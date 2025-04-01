document.addEventListener("DOMContentLoaded", function () {
    const pageSizeDropdown = document.getElementById("pageSize");
    if (!pageSizeDropdown) return; // Exit if pagination is not on this page

    const currentPageSize = document.getElementById("currentPageSize").value;

    // Set selected value in the dropdown
    for (let option of pageSizeDropdown.options) {
        if (option.value === currentPageSize) {
            option.selected = true;
            break;
        }
    }

    pageSizeDropdown.addEventListener("change", function () {
        const pageSize = this.value;
        const urlParams = new URLSearchParams(window.location.search);
        urlParams.set("pageSize", pageSize);
        urlParams.set("pageNumber", 1); // Reset to page 1 when changing size

        const newUrl = `${window.location.pathname}?${urlParams.toString()}`;
        window.location.href = newUrl;
    });
});

