
<?php
$email = $_POST["email"];
$num = $_POST["number"];

$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

$sql = "SELECT email,randomNum FROM playerTmp WHERE email ='".$email."'";
$result = $conn->query($sql) or die("2 : Name check query failed");

if($result->num_rows > 0){
  while($row = $result->fetch_assoc()) {
    $numCheck = $row["randomNum"];
    if($num == $numCheck){
        echo "successfully";
        $sql2 = "DELETE FROM playerTmp WHERE email ='".$email."'";
        $conn->query($sql2);
        $result2 = $conn->query($sql2);
      }
  }
}
$conn->close();
?>
