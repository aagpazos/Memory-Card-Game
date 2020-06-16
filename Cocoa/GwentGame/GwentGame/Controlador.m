//
//  Controlador.m
//  GwentGame
//
//  Created by Adrian on 19/10/2019.
//  Copyright © 2019 Adrian A. G. Pazos. All rights reserved.
//

#import "Controlador.h"
#import "Card.h"
#import "GameView.h"
#import "PanelController.h"
#import "Baraja.h"

@implementation Controlador

extern NSString *PanelChangeNotification;

-(id)init
{
    self = [super init];
    if(!self)
        return nil;
    baraja = [[Baraja alloc]init];
    panelController = [[PanelController alloc]initWithBaraja:baraja];
    cartasAbiertas = 0;
    jugando = NO;
    numeroCartas = 6;
    NSNotificationCenter *nc = [NSNotificationCenter defaultCenter];
    [nc addObserver:self
           selector:@selector(handlePanelChange:)
               name:PanelChangeNotification
             object:nil];
    return self;
}

-(void)dibujarBaraja:(NSRect)b withGraphicsContext:(NSGraphicsContext *)ctx
{
    for(Card *c in baraja.cartas){
        [c dibujarCarta:b withGraphicsContext:ctx];
    }
}

-(IBAction)empezarJuego:(id)sender
{
    [baraja.cartas removeAllObjects];
    cartasAbiertas = 0;
    jugando = YES;
    Card *card,*otherCard;
    for(int i=0; i<(numeroCartas/2); i++){
        card = [Card randomCard];
        [baraja addCard:card];
        otherCard = [Card copyCardWithOtherOrigin:card.imageName];
        [baraja addCard:otherCard];
        
    }
    [miVista setBackgroundName:@"background.jpg"];
    [miVista setNeedsDisplay:YES];
    [panelController reloadTableData];
    [boton setEnabled:NO];
    [refreshButton setEnabled:YES];
}

-(void)mouseEvent:(NSPoint)point withBounds:(NSRect)b
withGraphicsContext:(nonnull NSGraphicsContext *)ctx
{
    BOOL encontrado;
    Card *card1, *card2;
    NSInteger terceraCarta = -1;
    NSInteger nCard1 = -1, nCard2 = -1;
    
    if(cartasAbiertas < 3){
        for(int i = baraja.numCartasEnLaBaraja-1; i>=0; i--){
            Card *c = [baraja cardWithIndex: i];
            encontrado = [c mouseEvent:point withBounds:b withGraphicsContext:ctx];
            if(encontrado && !c.abierta){
                [baraja darVueltaCarta:c];
                cartasAbiertas++;
                if(cartasAbiertas == 3) terceraCarta = i;
                break;
            }
        }
    }
    
    if(cartasAbiertas == 2){
        NSInteger nCard1 = -1, nCard2 = -1;
    
        for(int i=0; i<baraja.numCartasEnLaBaraja; i++){
            Card *c = [baraja cardWithIndex:i];
            if(YES == c.abierta){
                if(nCard1 != -1 && nCard2 == -1 ) nCard2 = i;
                if(nCard1 == -1 && nCard2 == -1) nCard1 = i;
            }
        }
        card1 = [baraja cardWithIndex:nCard1];
        card2 = [baraja cardWithIndex:nCard2];
        
        if([card1.imageName isEqualToString:card2.imageName]){
            [baraja deleteCard:card1];
            [baraja deleteCard:card2];
            cartasAbiertas = 0;
        }
    }
    
    if(cartasAbiertas == 3){
        for(int i=0; i<baraja.numCartasEnLaBaraja; i++){
            Card *c = [baraja cardWithIndex: i];
            if(YES == c.abierta){
                if(nCard1 != -1 && nCard2 == -1 && i!=terceraCarta) nCard2 = i;
                if(nCard1 == -1 && nCard2 == -1 && i!=terceraCarta) nCard1 = i;
            }
        }
        card1 = [baraja cardWithIndex: nCard1];
        card2 = [baraja cardWithIndex: nCard2];
        [baraja darVueltaCarta:card1];
        [baraja darVueltaCarta:card2];

        cartasAbiertas = 1;
    }
    
    [miVista setNeedsDisplay:YES];
    [panelController reloadTableData];
    [self finJuego];
    
}


-(IBAction)showPanel:(id)sender
{
    [panelController showWindow:self];
}

-(void)reloadView{
    [miVista setNeedsDisplay:YES];
}


-(void)handlePanelChange:(NSNotification *) aNotification
{
    NSDictionary *notificacionInfo = [aNotification userInfo];
    NSString *quitarAbierta = [notificacionInfo objectForKey:@"quitarAbierta"];
    NSNumber *sliderValue = [notificacionInfo objectForKey:@"sliderValue"];
    if(quitarAbierta != nil){
        if([quitarAbierta isEqualToString:@"YES"]){
            cartasAbiertas--;
        }
        [miVista setNeedsDisplay:YES];
        [self finJuego];
    }
    
    if(sliderValue != nil){
        numeroCartas = [sliderValue integerValue];
    }
}

-(IBAction)empezarDeNuevo:(id)sender
{
    [self empezarJuego:sender];
}

-(void)finJuego
{
    if(baraja.numCartasEnLaBaraja == 0 && jugando){
        NSAlert *alert = [[NSAlert alloc]init];
        [alert setMessageText:@"You win!"];
        [alert setInformativeText:@"Ya no quedan mas cartas! Has ganado! ¿Quieres jugar de nuevo?"];
        [alert addButtonWithTitle:@"SI"];
        [alert addButtonWithTitle:@"NO"];
        [alert setIcon:[NSImage imageNamed:@"win.jpg"]];
        NSModalResponse respuesta = [alert runModal];
        jugando = NO;
        [boton setEnabled:YES];
        [refreshButton setEnabled:NO];
        if(respuesta == NSAlertFirstButtonReturn)
            [self empezarJuego:self];
        else{
            [miVista setBackgroundName:@"backgroundStart.jpg"];
            [miVista setNeedsDisplay:YES];
        }
    }
    
    if(baraja.numCartasEnLaBaraja == 1
       || (baraja.numCartasEnLaBaraja == 2 && ![[baraja cardWithIndex:0].imageName isEqualToString:[baraja cardWithIndex:1].imageName]))
    {
        if(!([baraja cardWithIndex:0].abierta))
        {
            [baraja darVueltaCarta:[baraja cardWithIndex:0]];
        }
        if(baraja.numCartasEnLaBaraja == 2 && !([baraja cardWithIndex:1].abierta))
        {
            [baraja darVueltaCarta:[baraja cardWithIndex:1]];
        }
        [miVista setNeedsDisplay:YES];
        NSAlert *alert = [[NSAlert alloc]init];
        [alert setMessageText:@"GAME OVER"];
        [alert setInformativeText:@"Has perdido! ¿Quieres jugar de nuevo?"];
        [alert addButtonWithTitle:@"SI"];
        [alert addButtonWithTitle:@"NO"];
        [alert setIcon:[NSImage imageNamed:@"gameover.png"]];
        NSModalResponse respuesta = [alert runModal];
        jugando = NO;
        cartasAbiertas = 0;
        [boton setEnabled:YES];
        [refreshButton setEnabled:NO];
        if(respuesta == NSAlertFirstButtonReturn)
            [self empezarJuego:self];
        else{
            [baraja.cartas removeAllObjects];
            [panelController reloadTableData];
            [miVista setBackgroundName:@"backgroundStart.jpg"];
            [miVista setNeedsDisplay:YES];
        }
    }
    
}


-(BOOL)windowShouldClose:(NSWindow *)sender
{
    NSAlert *alert = [[NSAlert alloc]init];
    [alert setMessageText:@"Atencion"];
    [alert setInformativeText:@"¿Estas seguro que desea cerrar la ventana?"];
    [alert addButtonWithTitle:@"NO"];
    [alert addButtonWithTitle:@"SI"];
    NSModalResponse respuesta = [alert runModal];
    if (respuesta == NSAlertFirstButtonReturn)
        return NO;
    else
        [NSApp terminate:self];
    return YES;
}
@end
