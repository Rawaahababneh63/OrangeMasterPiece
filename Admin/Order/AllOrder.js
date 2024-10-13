async function Allorders() {
    const url = "https://localhost:44332/api/Order/download-order";
    let response = await fetch(url);
    let data = await response.json();
    data.forEach(element => {
        let Orders = document.getElementById("getorder");
        Orders.innerHTML += `
          <tr>
                    <td>${element.user}</td>
                    <td>${element.email}</td>
                    <td>${element.status}</td>
                    <td>${element.amount}</td>
                    <td>${element.date}</td>
        </tr> 
         `
        });
    
}
Allorders();