<?php
$username = $_POST["username"];
$item = $_POST["item"];


$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

if($item === 'item1'){
  $sql = "SELECT item1 FROM item WHERE username ='".$username."'";
  $result = $conn->query($sql) or die("2 : Name check query failed");
  if($result->num_rows > 0){
    while($row = $result->fetch_assoc()) {
      //echo $row["hash"];
      echo $row["item1"];
    }
  }
}
else if($item === 'item2'){
  $sql = "SELECT item2 FROM item WHERE username ='".$username."'";
  $result = $conn->query($sql) or die("2 : Name check query failed");
  if($result->num_rows > 0){
    while($row = $result->fetch_assoc()) {
      //echo $row["hash"];
      echo $row["item2"];
    }
  }
}
else if($item === 'item3'){
  $sql = "SELECT item3 FROM item WHERE username ='".$username."'";
  $result = $conn->query($sql) or die("2 : Name check query failed");
  if($result->num_rows > 0){
    while($row = $result->fetch_assoc()) {
      //echo $row["hash"];
      echo $row["item3"];
    }
  }
}
else if($item === 'item4'){
  $sql = "SELECT item4 FROM item WHERE username ='".$username."'";
  $result = $conn->query($sql) or die("2 : Name check query failed");
  if($result->num_rows > 0){
    while($row = $result->fetch_assoc()) {
      //echo $row["hash"];
      echo $row["item4"];
    }
  }
}
$conn->close();
?>
