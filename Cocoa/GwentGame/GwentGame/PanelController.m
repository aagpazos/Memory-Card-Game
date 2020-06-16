//
//  PanelController.m
//  GwentGame
//
//  Created by Adrian on 18/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import "PanelController.h"
#import "Baraja.h"
#import "Controlador.h"

@interface PanelController ()

@end

@implementation PanelController
@synthesize baraja;

NSString *PanelChangeNotification = @"PanelChange";

-(id) init
{
    if(![super initWithWindowNibName:@"PanelController"])
        return nil;
    return self;
}


-(id) initWithWindow:(NSWindow *)window
{
    self = [super initWithWindow:window];
    if(self){
        
    }
    return self;
}

- (void)windowDidLoad {
    [super windowDidLoad];
}

-(id)initWithBaraja:(Baraja *)baraja{
    if (![self init])
        return nil;
    [self setBaraja:baraja];
    return self;
}

-(NSInteger)numberOfRowsInTableView:(NSTableView *)tableView
{
    return [baraja numCartasEnLaBaraja];
}

- (id)tableView:(NSTableView *)tableView objectValueForTableColumn:(NSTableColumn *)tableColumn row:(NSInteger)row
{
    Card *c = [baraja cardWithIndex: row];

    NSString *fila = [[NSString alloc]initWithFormat:@"%-20s \t\t Carta Abierta: %@ \t\t %.1fx%.1f",
                      [c.imageName UTF8String], c.abierta ? @"SI" : @"NO", c.rectangulo.origin.x, c.rectangulo.origin.y];
    return fila;
}

-(void)reloadTableData{
    [aTableView reloadData];
}

-(IBAction)buttonDelete:(id)sender{
    NSNotificationCenter *nc = [NSNotificationCenter defaultCenter];
    NSString *quitarAbierta = @"NO";
    NSInteger row = [aTableView selectedRow];
    Card *toDelete = [baraja cardWithIndex: row];
    if(toDelete.abierta == YES)
        quitarAbierta = @"YES";
    [baraja deleteCard:toDelete];
    [aTableView reloadData];
    NSDictionary *notficacionInfo = [NSDictionary dictionaryWithObject:quitarAbierta forKey:@"quitarAbierta"];
    [nc postNotificationName:PanelChangeNotification
                      object:self
                    userInfo:notficacionInfo];
    
}

- (void)tableViewSelectionDidChange:(NSNotification *)notification
{
    NSInteger row = [aTableView selectedRow];
    if(row == -1){
        [buttonDelete setEnabled:NO];
        return;
    }
    [buttonDelete setEnabled:YES];
}

-(IBAction)sliderAction:(id)sender
{
    int value = (int)[sender integerValue];
    NSNumber *valueSlider = [[NSNumber alloc]initWithInt:value];
    NSNotificationCenter *nc = [NSNotificationCenter defaultCenter];    NSDictionary *notificacionInfo = [NSDictionary dictionaryWithObject:valueSlider forKey:@"sliderValue"];
    [nc postNotificationName:PanelChangeNotification
                      object:self
                    userInfo:notificacionInfo];
    [textField setIntValue:value];
}

-(IBAction)info:(id)sender
{
    NSAlert *alert = [[NSAlert alloc]init];
    [alert setMessageText:@"Informacion"];
    [alert setInformativeText:@"Hecho por: Adrian Antonio Gonzalez Pazos \nPara la asignatura: Interfaces Graficas de Usuario \nGwent y The Witcher son marcas registradas por CD Projekt, Netflix y Andrzej Sapkowski, y su uso ha sido con fines educativos"];
    [alert addButtonWithTitle:@"OK"];
    [alert setIcon:[NSImage imageNamed:@"info.jpg"]];
    [alert runModal];
    
}

@end
