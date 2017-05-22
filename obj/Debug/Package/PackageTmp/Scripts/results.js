        function turnOnOffDetail(row) {
            var rowIndex = row.parentElement.parentElement.rowIndex;
            var gridView = document.getElementById("MainContent_gvResults");
            var rowType = gridView.rows(rowIndex).cells(1).textContent;
            switch (rowType) {
                case "Total N (lbs/ac)":
                    gridView.rows(rowIndex+1).style.display="";
                    gridView.rows(rowIndex + 2).style.display = "";
                    gridView.rows(rowIndex + 3).style.display = "";
                    gridView.rows(rowIndex + 4).style.display = "";
                    break;
                case "Total P (lbs/ac)":
                    gridView.rows(8).style.display = "";
                    gridView.rows(9).style.display = "";
                    gridView.rows(10).style.display = "";
                    break;
                case "Total Flow (in)":
                    gridView.rows(12).style.display = "";
                    gridView.rows(13).style.display = "";
                    gridView.rows(14).style.display = "";
                    break;
                case "Other Water Information":
                    gridView.rows(16).style.display = "";
                    gridView.rows(17).style.display = "";
                    break;
                case "Sediment (t/ac)":
                    break;
                case "Crop Yield":
                    for (i = rowIndex + 1; i < gridView.rows; i++) {
                        gridView.rows(i).style.display = "";
                    }
                    break;
            }
        }
